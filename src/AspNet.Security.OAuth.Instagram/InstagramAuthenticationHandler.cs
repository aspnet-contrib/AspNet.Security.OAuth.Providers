/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Diagnostics;
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
using Microsoft.AspNet.WebUtilities;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Instagram {
    public class InstagramAuthenticationHandler : OAuthHandler<InstagramAuthenticationOptions> {
        public InstagramAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);

            if (Options.UseSignedRequests) {
                // Compute the HMAC256 signature.
                var signature = ComputeSignature(address);

                // Add the signature to the query string.
                address = QueryHelpers.AddQueryString(address, "sig", signature);
            }

            var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
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

        protected virtual string ComputeSignature(string address) {
            using (var algorithm = new HMACSHA256(Encoding.UTF8.GetBytes(Options.ClientSecret))) {
                var query = new UriBuilder(address).Query;

                // Extract the parameters from the query string.
                var parameters = (from parameter in QueryHelpers.ParseQuery(query)
                                  orderby parameter.Key
                                  select $"{parameter.Key}={parameter.Value}").ToArray();
                Debug.Assert(parameters.Length != 0);

                // See https://www.instagram.com/developer/secure-api-requests/
                // for more information about the signature format.
                var bytes = Encoding.UTF8.GetBytes($"/users/self|{string.Join("|", parameters)}");

                // Compute the HMAC256 signature.
                var hash = algorithm.ComputeHash(bytes);

                // Convert the hash to its lowercased hexadecimal representation.
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}