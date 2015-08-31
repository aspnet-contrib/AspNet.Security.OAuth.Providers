﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.Framework.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Yahoo {
    public class YahooAuthenticationHandler : OAuthAuthenticationHandler<YahooAuthenticationOptions> {
        public YahooAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {}

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, YahooAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Name, YahooAuthenticationHelper.GetNickname(payload), Options.ClaimsIssuer)
                .AddOptionalClaim("urn:yahoo:familyname", YahooAuthenticationHelper.GetFamilyName(payload), Options.ClaimsIssuer)
                .AddOptionalClaim("urn:yahoo:givenname", YahooAuthenticationHelper.GetGivenName(payload), Options.ClaimsIssuer)
                .AddOptionalClaim("urn:yahoo:profile", YahooAuthenticationHelper.GetProfileUrl(payload), Options.ClaimsIssuer)
                .AddOptionalClaim("urn:yahoo:profileimage", YahooAuthenticationHelper.GetProfileImageUrl(payload), Options.ClaimsIssuer);

            var notification = new OAuthAuthenticatedContext(Context, Options, Backchannel, tokens, payload) {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            await Options.Notifications.Authenticated(notification);

            if (notification.Principal?.Identity == null) {
                return null;
            }

            return new AuthenticationTicket(
                notification.Principal, notification.Properties,
                notification.Options.AuthenticationScheme);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri) {
            var tokenRequestParameters = new Dictionary<string, string>() {
                {"redirect_uri", redirectUri},
                {"code", code},
                {"grant_type", "authorization_code"},
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Add Yahoo BASIC auth header which is generated through a Base64 encoding of client_id:client_secret per RFC 2617
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Options.ClientId}:{Options.ClientSecret}"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            requestMessage.Content = requestContent;
            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
            response.EnsureSuccessStatusCode();
            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            return new OAuthTokenResponse(payload);
        }
    }
}