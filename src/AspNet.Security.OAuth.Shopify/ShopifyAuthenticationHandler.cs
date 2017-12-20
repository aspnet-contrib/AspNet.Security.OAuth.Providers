/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Shopify
{
    /// <inheritdoc />
    [UsedImplicitly]
    public class ShopifyAuthenticationHandler : OAuthHandler<ShopifyAuthenticationOptions>
    {
        /// <inheritdoc />
        public ShopifyAuthenticationHandler(
            [NotNull] IOptionsMonitor<ShopifyAuthenticationOptions> options, 
            [NotNull] ILoggerFactory logger, 
            [NotNull] UrlEncoder encoder, 
            [NotNull] ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, 
            [NotNull] OAuthTokenResponse tokens)
        {
            var uri = string.Format(ShopifyAuthenticationDefaults.UserInformationEndpoint,
                properties.Items[ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty]);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("X-Shopify-Access-Token", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());
            
                throw new HttpRequestException("An error occurred while retrieving the shop profile.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

#if DEBUG
            // ReSharper disable once UnusedVariable
            var jsonStr = payload.ToString(Formatting.Indented);
#endif

            // In Shopify, the customer can modify the scope given to the app. Apps should verify
            // that the customer is allowing the required scope.
            var actualScope = tokens.Response["scope"].ToString();
            var isPersistent = true;

            // If the request was for a "per-user" (i.e. no offline access)
            if (tokens.Response.TryGetValue("expires_in", out var val))
            {
                isPersistent = false;

                var expires = DateTimeOffset.Now.AddSeconds(Convert.ToInt32(val.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Expiration, expires.ToString("O"), ShopifyAuthenticationOptions.XmlSchemaDateTime));

                actualScope = tokens.Response["associated_user_scope"].ToString();
                
                var userData = tokens.Response["associated_user"].ToString();
                identity.AddClaim(new Claim(ClaimTypes.UserData, userData));
            }

            identity.AddClaim(new Claim(ClaimTypes.IsPersistent, isPersistent.ToString(), ShopifyAuthenticationOptions.XmlSchemaBoolean));
            identity.AddClaim(new Claim(ShopifyAuthenticationDefaults.ShopifyScopeClaimType, actualScope));

            var principal = new ClaimsPrincipal(identity);

            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);
            var ticket = new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);

            return ticket;
        }

        /// <inheritdoc />
        protected override string FormatScope()
        {
            return string.Join(",", Options.Scope);
        }

        /// <inheritdoc />
        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            if (!properties.Items.ContainsKey(ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty))
            {
                var msg =
                    $"Shopify provider AuthenticationProperties must contain {ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty}";

                Logger.LogError(msg);
                throw new Exception(msg);
            }

            var shopName = properties.Items[ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty];
            var uri = string.Format(Options.AuthorizationEndpoint, shopName);

            // Get the permission scope, which can either be set in options or overridden in AuthenticationProperties.
            var scope = properties.Items.ContainsKey(ShopifyAuthenticationDefaults.ShopScopeAuthenticationProperty) ? 
                properties.Items[ShopifyAuthenticationDefaults.ShopScopeAuthenticationProperty] : 
                FormatScope();

            var url = QueryHelpers.AddQueryString(uri, new Dictionary<string, string>
            {
                ["client_id"] = Options.ClientId,
                ["scope"] = scope,
                ["redirect_uri"] = redirectUri,
                ["state"] = Options.StateDataFormat.Protect(properties)
            });

            // If we're requesting a per-user, online only, token, add the grant_options query param.
            if (properties.Items.ContainsKey(ShopifyAuthenticationDefaults.GrantOptionsAuthenticationProperty))
            {
                var grantOptions = properties.Items[ShopifyAuthenticationDefaults.GrantOptionsAuthenticationProperty];
                if (grantOptions != null &&
                    grantOptions.Equals(ShopifyAuthenticationDefaults.PerUserAuthenticationPropertyValue))
                {
                    url = QueryHelpers.AddQueryString(url, "grant_options[]",
                        ShopifyAuthenticationDefaults.PerUserAuthenticationPropertyValue);
                }
            }

            return url;
        }

        /// <inheritdoc />
        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(
            [NotNull] string code, 
            [NotNull] string redirectUri)
        {
            string shopDns;

            try
            {
                var shop = Context.Request.Query["shop"];
                var state = Context.Request.Query["state"];

                // Shop name must end with myshopify.com
                if (!shop.ToString().EndsWith(".myshopify.com"))
                {
                    throw new Exception("Unexpected query string.");    
                }

                // Strip out the "myshopify.com" suffix
                shopDns = shop.ToString().Split('.').First();

                // Verify that the shop name encoded in "state" matches the shop name we used to
                // request the token. This probably isn't necessary, but it's an easy extra verification.
                var z = Options.StateDataFormat.Unprotect(state);
                if (!z.Items[ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty]
                    .Equals(shopDns, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new Exception("Unexpected query string");
                }
            }
            catch (Exception e)
            {
                Logger.LogError("An error occurred while exchanging tokens: " + e.Message);
                return OAuthTokenResponse.Failed(e);
            }

            var uri = string.Format(Options.TokenEndpoint, shopDns);
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = Options.ClientId,
                ["client_secret"] = Options.ClientSecret,
                ["code"] = code
            });

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);

            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                    /* Status: */ response.StatusCode,
                    /* Headers: */ response.Headers.ToString(),
                    /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            return OAuthTokenResponse.Success(payload);
        }
    }
}