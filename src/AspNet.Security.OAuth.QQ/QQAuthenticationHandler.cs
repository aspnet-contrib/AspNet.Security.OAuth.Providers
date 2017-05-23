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
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using JetBrains.Annotations;

namespace AspNet.Security.OAuth.QQ
{
    public class QQAuthenticationHandler : OAuthHandler<QQAuthenticationOptions>
    {
        public QQAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var openid = await GetUserOpenIdAsync(tokens);
            var queryString = new Dictionary<string, string>()
            {
                {"oauth_consumer_key",Options.ClientId },
                {"access_token",tokens.AccessToken },
                {"openid",openid },
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
            var ret = payload.Value<int>("ret");
            if (ret != 0)
            {
                var msg = payload.Value<string>("msg");
                throw new HttpRequestException($"An error occurred when retrieving user information. code: {ret}, msg: {msg}");
            }

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, openid, Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, QQAuthenticationHelper.GetNickname(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Gender, QQAuthenticationHelper.GetGender(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:qq:figureurl", QQAuthenticationHelper.GetFigureUrl(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:qq:figureurl_1", QQAuthenticationHelper.GetFigureUrl1(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:qq:figureurl_2", QQAuthenticationHelper.GetFigureUrl2(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:qq:figureurl_qq_1", QQAuthenticationHelper.GetFigureQQUrl1(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:qq:figureurl_qq_2", QQAuthenticationHelper.GetFigureQQUrl2(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var queryString = new Dictionary<string, string>()
            {
                {"client_id",Options.ClientId},
                {"client_secret",Options.ClientSecret },
                {"redirect_uri",redirectUri },
                {"code",code },
                {"grant_type","authorization_code" },
            };
            var endpoint = QueryHelpers.AddQueryString(Options.TokenEndpoint, queryString);
            var response = await Backchannel.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                var error = "OAuth token endpoint failure: " + await response.Content.ReadAsStringAsync();
                return OAuthTokenResponse.Failed(new Exception(error));
            }
            var payload = JObject.FromObject(QueryHelpers.ParseQuery(await response.Content.ReadAsStringAsync()).ToDictionary(k => k.Key, k => k.Value.ToString()));
            return OAuthTokenResponse.Success(payload);
        }

        private async Task<string> GetUserOpenIdAsync(OAuthTokenResponse tokens)
        {
            var endpoint = QueryHelpers.AddQueryString(Options.OpenIdEndpoint, "access_token", tokens.AccessToken);
            var response = await Backchannel.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred while retrieving the user openid.({response.StatusCode})");
            }
            var body = await response.Content.ReadAsStringAsync();
            var i = body.IndexOf("{");
            if (i > 0)
            {
                var j = body.LastIndexOf("}");
                body = body.Substring(i, j - i + 1);
            }
            var payload = JObject.Parse(body);
            var openid = payload.Value<string>("openid");
            if (string.IsNullOrEmpty(openid))
            {
                throw new HttpRequestException("An error occurred while retrieving the user openid. the openid is null.");
            }
            return openid;
        }

        protected override string FormatScope() => string.Join(",", Options.Scope);
    }
}
