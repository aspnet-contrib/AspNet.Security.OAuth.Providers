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
using Microsoft.AspNet.WebUtilities;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Instagram {
    public class InstagramAuthenticationHandler : OAuthHandler<InstagramAuthenticationOptions> {
        public InstagramAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {}

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            var userInformationEndpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, name: "access_token", value: tokens.AccessToken);

            if (Options.UseSignedRequests) {
                var signature = ComputeSignature(userInformationEndpoint);

                userInformationEndpoint = QueryHelpers.AddQueryString(userInformationEndpoint, name: "sig",
                    value: signature);
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

        /// <summary>
        /// See https://www.instagram.com/developer/secure-api-requests/
        /// for more information about the signature format
        /// </summary>
        protected virtual string ComputeSignature(string address) {
            using (var algorithm = new HMACSHA256(Encoding.UTF8.GetBytes(Options.ClientSecret))) {
                // Select only the parameter part of the url
                var query = address.Substring(address.IndexOf("?", StringComparison.Ordinal));

                // Extract parameters from the query string
                var parameters = from parameter in QueryHelpers.ParseQuery(query)
                    orderby parameter.Key
                    select $"|{parameter.Key}={parameter.Value}";

                var bytes = Encoding.UTF8.GetBytes($"/users/self{String.Join("", parameters)}");

                var hash = algorithm.ComputeHash(bytes);

                // Convert the hash to its lowercased hexadecimal representation.
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}