﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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
        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            if (Options.AuthorizationEndpoint != WeixinAuthenticationDefaults.AuthorizationEndpoint)
            {
                var oauthstate = Request.Query["oauthstate"];
                Request.Query = new QueryCollection(Request.Query.Select(c =>
                {
                    if (c.Key == "state")
                        return new KeyValuePair<string, StringValues>("state", oauthstate);
                    return c;
                }).ToDictionary(c => c.Key, c => c.Value));
            }
            return await base.HandleRemoteAuthenticateAsync();
        }
        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity, [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string>
            {
                ["access_token"] = tokens.AccessToken,
                ["openid"] = tokens.Response.Value<string>("openid")
            });

            var response = await Backchannel.GetAsync(address);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            if (!string.IsNullOrEmpty(payload.Value<string>("errcode")))
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, new Dictionary<string, string>()
            {
                ["appid"] = Options.ClientId,
                ["secret"] = Options.ClientSecret,
                ["code"] = code,
                ["grant_type"] = "authorization_code"
            });

            var response = await Backchannel.GetAsync(address);
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
            if (!string.IsNullOrEmpty(payload.Value<string>("errcode")))
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }
            return OAuthTokenResponse.Success(payload);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var oauthstate = Options.StateDataFormat.Protect(properties);
            if (Options.AuthorizationEndpoint != WeixinAuthenticationDefaults.AuthorizationEndpoint)
            {
                //Store state in redirectUri when authorizing Wechat Web pages to prevent "too long state parameters" error
                redirectUri = redirectUri.Contains("?") ? $"{redirectUri}&{nameof(oauthstate)}={oauthstate}" : $"{redirectUri}?{nameof(oauthstate)}={oauthstate}";
                oauthstate = "#wechat_redirect";//The Parameters Necessary for Web Authorization of Wechat
            }
            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
            {
                ["appid"] = Options.ClientId,
                ["scope"] = FormatScope(),
                ["response_type"] = "code",
                ["redirect_uri"] = redirectUri,
                ["state"] = oauthstate
            });
        }

        protected override string FormatScope() => string.Join(",", Options.Scope);
    }
}
