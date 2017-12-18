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
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Shopify
{
    public class ShopifyAuthenticationHandler : OAuthHandler<ShopifyAuthenticationOptions>
    {
        public ShopifyAuthenticationHandler(
            [NotNull] IOptionsMonitor<ShopifyAuthenticationOptions> options, 
            [NotNull] ILoggerFactory logger, 
            [NotNull] UrlEncoder encoder, 
            [NotNull] ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, 
            [NotNull] OAuthTokenResponse tokens)
        {
            var uri = string.Format(ShopifyAuthenticationDefaults.UserInformationEndpoint + "/shop",
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

            var actualScope = tokens.Response["scope"].ToString();
            identity.AddClaim(new Claim("urn:shopify:scope", actualScope));

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);

            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);
            var ticket = new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
            return ticket;
        }

        protected override string FormatScope()
        {
            return Options.ShopifyScope;
        }

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

            var url = QueryHelpers.AddQueryString(uri, new Dictionary<string, string>
            {
                ["client_id"] = Options.ApiKey ?? Options.ClientId,
                ["scope"] = FormatScope(),
                ["redirect_uri"] = redirectUri,
                ["state"] = Options.StateDataFormat.Protect(properties),
            });

            return url;
        }

        
        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(
            [NotNull] string code, 
            [NotNull] string redirectUri)
        {
            var shop = Context.Request.Query["shop"];
            var hmac = Context.Request.Query["hmac"];
            var state = Context.Request.Query["state"];

            var shopDns = shop.ToString().Split('.').First();
            var z = Options.StateDataFormat.Unprotect(state);
            if (!z.Items[ShopifyAuthenticationDefaults.ShopNameAuthenticationProperty]
                .Equals(shopDns, StringComparison.InvariantCultureIgnoreCase))
            {
                
            }

            var uri = string.Format(Options.TokenEndpoint, shopDns);
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = Options.ApiKey ?? Options.ClientId,
                ["client_secret"] = Options.ApiSecretKey ?? Options.ClientSecret,
                ["code"] = code,
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