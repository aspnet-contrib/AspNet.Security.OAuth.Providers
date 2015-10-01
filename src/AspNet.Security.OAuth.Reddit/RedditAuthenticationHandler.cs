/*
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
using Microsoft.AspNet.WebUtilities;
using Microsoft.Framework.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Reddit {
    public class RedditAuthenticationHandler : OAuthHandler<RedditAuthenticationOptions> {
        public RedditAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, RedditAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, RedditAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:reddit:over18", RedditAuthenticationHelper.GetOver18(payload), Options.ClaimsIssuer);

            var context = new OAuthCreatingTicketContext(Context, Options, Backchannel, tokens, payload) {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            await Options.Events.CreatingTicket(context);

            if (context.Principal?.Identity == null) {
                return null;
            }

            return new AuthenticationTicket(context.Principal, context.Properties, Options.AuthenticationScheme);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri) {
            var address = base.BuildChallengeUrl(properties, redirectUri);

            // Add duration=permanent to the authorization request to get an access token that doesn't expire after 1 hour.
            // See https://github.com/reddit/reddit/wiki/OAuth2#authorization.
            return QueryHelpers.AddQueryString(address, name: "duration", value: "permanent");
        }

        protected override string FormatScope() {
            // Note: Reddit requires a non-standard comma-separated scope.
            // See https://github.com/reddit/reddit/wiki/OAuth2#authorization
            // and http://tools.ietf.org/html/rfc6749#section-3.3.
            return string.Join(",", Options.Scope);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] string code, [NotNull] string redirectUri) {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(Options.ClientId + ":" + Options.ClientSecret));

            var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                ["grant_type"] = "authorization_code",
                ["redirect_uri"] = redirectUri,
                ["code"] = code
            });

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            return new OAuthTokenResponse(payload);
        }
    }
}