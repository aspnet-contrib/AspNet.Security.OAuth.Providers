/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Weixin
{
    public class WeixinAuthenticationHandler : OAuthHandler<WeixinAuthenticationOptions>
    {
        public WeixinAuthenticationHandler(
            [NotNull] IOptionsMonitor<WeixinAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        private const string OauthState = "_oauthstate";
        private const string State = "state";

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            if (!IsWeixinAuthorizationEndpointInUse())
            {
                if (Request.Query.TryGetValue(OauthState, out var stateValue))
                {
                    var query = Request.Query.ToDictionary(c => c.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);
                    if (query.TryGetValue(State, out _))
                    {
                        query[State] = stateValue;
                        Request.QueryString = QueryString.Create(query);
                    }
                }
            }

            return await base.HandleRemoteAuthenticateAsync();
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            string address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string?>
            {
                ["access_token"] = tokens.AccessToken,
                ["openid"] = tokens.Response?.RootElement.GetString("openid")
            });

            using var response = await Backchannel.GetAsync(address);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
            if (!string.IsNullOrEmpty(payload.RootElement.GetString("errcode")))
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            string address = QueryHelpers.AddQueryString(Options.TokenEndpoint, new Dictionary<string, string?>()
            {
                ["appid"] = Options.ClientId,
                ["secret"] = Options.ClientSecret,
                ["code"] = context.Code,
                ["grant_type"] = "authorization_code"
            });

            using var response = await Backchannel.GetAsync(address);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
            if (!string.IsNullOrEmpty(payload.RootElement.GetString("errcode")))
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            return OAuthTokenResponse.Success(payload);
        }

        protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
        {
            string stateValue = Options.StateDataFormat.Protect(properties);
            bool addRedirectHash = false;

            if (!IsWeixinAuthorizationEndpointInUse())
            {
                // Store state in redirectUri when authorizing Wechat Web pages to prevent "too long state parameters" error
                redirectUri = QueryHelpers.AddQueryString(redirectUri, OauthState, stateValue);
                addRedirectHash = true;
            }

            redirectUri = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string?>
            {
                ["appid"] = Options.ClientId,
                ["scope"] = FormatScope(),
                ["response_type"] = "code",
                ["redirect_uri"] = redirectUri,
                [State] = addRedirectHash ? OauthState : stateValue
            });

            if (addRedirectHash)
            {
                // The parameters necessary for Web Authorization of Wechat
                redirectUri += "#wechat_redirect";
            }

            return redirectUri;
        }

        protected override string FormatScope() => string.Join(',', Options.Scope);

        private bool IsWeixinAuthorizationEndpointInUse()
        {
            return string.Equals(Options.AuthorizationEndpoint, WeixinAuthenticationDefaults.AuthorizationEndpoint, StringComparison.OrdinalIgnoreCase);
        }
    }
}
