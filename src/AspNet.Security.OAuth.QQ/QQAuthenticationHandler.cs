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
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.QQ
{
    public class QQAuthenticationHandler : OAuthHandler<QQAuthenticationOptions>
    {
        public QQAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var identifier = await GetUserIdentifierAsync(tokens);
            if (string.IsNullOrEmpty(identifier))
            {
                throw new HttpRequestException("An error occurred while retrieving the user identifier.");
            }

            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string>
            {
                ["oauth_consumer_key"] = Options.ClientId,
                ["access_token"] = tokens.AccessToken,
                ["openid"] = identifier,
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

            var status = payload.Value<int>("ret");
            if (status != 0)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following message: {Message}.",
                                /* Status: */ status,
                                /* Message: */ payload.Value<string>("msg"));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            identity.AddOptionalClaim("urn:qq:picture", QQAuthenticationHelper.GetPicture(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:qq:picture_medium", QQAuthenticationHelper.GetPictureMedium(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:qq:picture_full", QQAuthenticationHelper.GetPictureFull(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:qq:avatar", QQAuthenticationHelper.GetAvatar(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:qq:avatar_full", QQAuthenticationHelper.GetAvatarFull(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] string code, [NotNull] string redirectUri)
        {
            var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, new Dictionary<string, string>()
            {
                ["client_id"] = Options.ClientId,
                ["client_secret"] = Options.ClientSecret,
                ["redirect_uri"] = redirectUri,
                ["code"] = code,
                ["grant_type"] = "authorization_code",
            });

            var request = new HttpRequestMessage(HttpMethod.Get, address);

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            var payload = JObject.FromObject(QueryHelpers.ParseQuery(await response.Content.ReadAsStringAsync())
                .ToDictionary(pair => pair.Key, k => k.Value.ToString()));

            return OAuthTokenResponse.Success(payload);
        }

        private async Task<string> GetUserIdentifierAsync(OAuthTokenResponse tokens)
        {
            var address = QueryHelpers.AddQueryString(Options.UserIdentificationEndpoint, "access_token", tokens.AccessToken);
            var request = new HttpRequestMessage(HttpMethod.Get, address);

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user identifier: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user identifier.");
            }

            var body = await response.Content.ReadAsStringAsync();

            var index = body.IndexOf("{");
            if (index > 0)
            {
                body = body.Substring(index, body.LastIndexOf("}") - index + 1);
            }

            var payload = JObject.Parse(body);

            return payload.Value<string>("openid");
        }

        protected override string FormatScope() => string.Join(",", Options.Scope);
    }
}
