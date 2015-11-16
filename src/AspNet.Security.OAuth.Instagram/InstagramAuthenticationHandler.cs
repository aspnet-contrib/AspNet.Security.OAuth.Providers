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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Instagram {
    public class InstagramAuthenticationHandler : OAuthHandler<InstagramAuthenticationOptions> {
        public InstagramAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {}

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            var userInformationEndpoint = $"{Options.UserInformationEndpoint}?access_token={tokens.AccessToken}";

            if (Options.SignedRequestsEnforced) {
                var parameters = new Dictionary<string, string> {
                    ["access_token"] = tokens.AccessToken
                };

                var sig = SignRequest(InstagramAuthenticationDefaults.UserInformationEndpoint,
                    parameters, Options.ClientSecret);

                userInformationEndpoint += $"&sig={sig}";
            }

            var request = new HttpRequestMessage(HttpMethod.Get, userInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response =
                await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync()).Value<JObject>("data");

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, InstagramAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, InstagramAuthenticationHelper.GetFullName(payload), Options.ClaimsIssuer);

            var context = new OAuthCreatingTicketContext(Context, Options, Backchannel, tokens, payload) {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            await Options.Events.CreatingTicket(context);

            if (context.Principal?.Identity == null) {
                return null;
            }

            return new AuthenticationTicket(context.Principal, context.Properties, context.Options.AuthenticationScheme);
        }

        private static byte[] CreateToken([NotNull] string endpoint, [NotNull] Dictionary<string, string> parameters) {
            var token = parameters.OrderBy(par => par.Key)
                .Aggregate(endpoint, (current, par) => current + $"|{par.Key}={par.Value}");
            return Encoding.UTF8.GetBytes(token);
        }

        private static string ConvertToHex(byte[] hash) {
            return BitConverter.ToString(hash).Replace("-", "");
        }

        private static string SignRequest([NotNull] string endpoint, [NotNull] Dictionary<string, string> parameters,
            string secret) {
            var token = CreateToken(endpoint, parameters);
            var key = Encoding.UTF8.GetBytes(secret);
            using (var hmac = new HMACSHA256(key)) {
                var hash = hmac.ComputeHash(token);
                return ConvertToHex(hash).ToLower();
            }
        }
    }
}