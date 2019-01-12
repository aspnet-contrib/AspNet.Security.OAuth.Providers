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
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.WeChat
{
    public class WeChatAuthenticationHandler : OAuthHandler<WeChatAuthenticationOptions>
    {
        public WeChatAuthenticationHandler(
            [NotNull] IOptionsMonitor<WeChatAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity, [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var payload = new JObject();
            switch (Options.Scope.SingleOrDefault())
            {
                case "snsapi_base":
                    payload["openid"] = tokens.Response.Value<string>("openid");
                    break;
                case "snsapi_userinfo":
                    var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint,
                        new Dictionary<string, string>
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

                    payload = JObject.Parse(await response.Content.ReadAsStringAsync());
                    if (!string.IsNullOrEmpty(payload.Value<string>("errcode")))
                    {
                        Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                        "returned a {Status} response with the following payload: {Headers} {Body}.",
                            /* Status: */ response.StatusCode,
                            /* Headers: */ response.Headers.ToString(),
                            /* Body: */ await response.Content.ReadAsStringAsync());

                        throw new HttpRequestException("An error occurred while retrieving user information.");
                    }

                    break;
                default:
                    throw new InvalidOperationException("Invalid scope");
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options,
                Backchannel, tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, new Dictionary<string, string>
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
            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
            {
                { "appid", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "response_type", "code" },
                { "scope", FormatScope() },
                { "state", Options.StateDataFormat.Protect(properties) }
            }) + "#wechat_redirect" /*required*/;
        }

        protected override string FormatScope()
        {
            if (Options.Scope.Count > 1)
            {
                throw new InvalidOperationException("WeChat MP OAuth does not accept multiple scopes");
            }

            return Options.Scope.SingleOrDefault();
        }
    }
}
