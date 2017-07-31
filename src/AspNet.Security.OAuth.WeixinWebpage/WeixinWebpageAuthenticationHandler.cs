/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace AspNet.Security.OAuth.WeixinWebpage
{
    public class WeixinWebpageAuthenticationHandler : OAuthHandler<WeixinWebpageAuthenticationOptions>
    {
        public WeixinWebpageAuthenticationHandler([NotNull] HttpClient client) : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            switch (Options.Scope.SingleOrDefault())
            {
                case "snsapi_base":
                    identity.AddOptionalClaim("urn:weixin:openid", tokens.Response.Value<string>("openid"), Options.ClaimsIssuer);
                    break;
                case "snsapi_userinfo":
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

                    identity.AddOptionalClaim(ClaimTypes.NameIdentifier, WeixinWebpageAuthenticationHelper.GetUnionid(payload), Options.ClaimsIssuer)
                            .AddOptionalClaim(ClaimTypes.Name, WeixinWebpageAuthenticationHelper.GetNickname(payload), Options.ClaimsIssuer)
                            .AddOptionalClaim(ClaimTypes.Gender, WeixinWebpageAuthenticationHelper.GetSex(payload), Options.ClaimsIssuer)
                            .AddOptionalClaim(ClaimTypes.Country, WeixinWebpageAuthenticationHelper.GetCountry(payload), Options.ClaimsIssuer)
                            .AddOptionalClaim("urn:weixin:openid", WeixinWebpageAuthenticationHelper.GetOpenId(payload), Options.ClaimsIssuer)
                            .AddOptionalClaim("urn:weixin:province", WeixinWebpageAuthenticationHelper.GetProvince(payload), Options.ClaimsIssuer)
                            .AddOptionalClaim("urn:weixin:city", WeixinWebpageAuthenticationHelper.GetCity(payload), Options.ClaimsIssuer)
                            .AddOptionalClaim("urn:weixin:headimgurl", WeixinWebpageAuthenticationHelper.GetHeadimgUrl(payload), Options.ClaimsIssuer)
                            .AddOptionalClaim("urn:weixin:privilege", WeixinWebpageAuthenticationHelper.GetPrivilege(payload), Options.ClaimsIssuer);
                    break;
                default:
                    throw new InvalidOperationException("Invalid scope");
            }
            
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens);

            await Options.Events.CreatingTicket(context);
            return context.Ticket;
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
            var scope = FormatScope();
            var state = Options.StateDataFormat.Protect(properties);

            var parameters = new Dictionary<string, string>
            {
                { "appid", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "response_type", "code" },
                { "scope", scope },
                { "state", state },
            };
            //The order of parameters is required to be correct

            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, parameters)
                + "#wechat_redirect" /*required*/;
        }

        protected override string FormatScope()
        {
            if (Options.Scope.Count > 1)
            {
                throw new InvalidOperationException("Weixin Webpage OAuth cannot have multiple scopes");
            }

            return Options.Scope.SingleOrDefault();
        }
    }
}
