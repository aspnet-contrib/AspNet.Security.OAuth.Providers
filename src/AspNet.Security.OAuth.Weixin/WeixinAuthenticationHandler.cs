/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using AspNet.Security.OAuth.Extensions;
using Newtonsoft.Json.Linq;
using JetBrains.Annotations;

namespace AspNet.Security.OAuth.Weixin
{
    public class WeixinAuthenticationHandler : OAuthHandler<WeixinAuthenticationOptions>
    {
        public WeixinAuthenticationHandler([NotNull] HttpClient client) : base(client) { }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity, [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var queryString = new Dictionary<string, string>()
            {
                {"access_token",tokens.AccessToken },
                {"openid",tokens.Response.Value<string>("openid") }
            };
            var endpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, queryString);
            var response = await Backchannel.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred when retrieving user information.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            // checking a remote server error response.
            if (!string.IsNullOrEmpty(payload.Value<string>("errcode")))
            {
                Logger.LogError($"retrieving user info failure: {payload.ToString()}");

                throw new HttpRequestException("An error occurred when retrieving user information.");
            }

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, WeixinAuthenticationHelper.GetUnionid(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, WeixinAuthenticationHelper.GetNickname(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Gender, WeixinAuthenticationHelper.GetSex(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Country, WeixinAuthenticationHelper.GetCountry(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weixin:openid", WeixinAuthenticationHelper.GetOpenId(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weixin:province", WeixinAuthenticationHelper.GetProvince(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weixin:city", WeixinAuthenticationHelper.GetCity(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weixin:headimgurl", WeixinAuthenticationHelper.GetHeadimgUrl(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weixin:privilege", WeixinAuthenticationHelper.GetPrivilege(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens);

            await Options.Events.CreatingTicket(context);
            return context.Ticket;
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var queryString = new Dictionary<string, string>()
            {
                {"appid",Options.ClientId },
                {"secret",Options.ClientSecret },
                {"code",code },
                {"grant_type","authorization_code" }
            };
            var endpoint = QueryHelpers.AddQueryString(Options.TokenEndpoint, queryString);
            var response = await Backchannel.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                var error = "OAuth token endpoint failure: " + await response.Content.ReadAsStringAsync();
                return OAuthTokenResponse.Failed(new Exception(error));
            }
            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            // checking a remote server error response.
            if (!string.IsNullOrEmpty(payload.Value<string>("errcode")))
            {
                var error = "OAuth token endpoint failure: " + payload.ToString();
                return OAuthTokenResponse.Failed(new Exception(error));
            }
            return OAuthTokenResponse.Success(payload);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var scope = FormatScope();
            var state = Options.StateDataFormat.Protect(properties);

            var queryString = new Dictionary<string, string>()
            {
                {"appid",Options.ClientId },
                { "scope", scope },
                { "response_type", "code" },
                { "redirect_uri", redirectUri },
                { "state", state }
            };
            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, queryString);
        }

        protected override string FormatScope()
        {
            return string.Join(",", Options.Scope);
        }
    }
}
