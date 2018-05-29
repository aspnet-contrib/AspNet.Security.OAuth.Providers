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
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Instagram
{
    public class InstagramAuthenticationHandler : OAuthHandler<InstagramAuthenticationOptions>
    {
        public InstagramAuthenticationHandler(
            [NotNull] IOptionsMonitor<InstagramAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);

            if (Options.UseSignedRequests)
            {
                // Compute the HMAC256 signature.
                var signature = ComputeSignature(address);

                // Add the signature to the query string.
                address = QueryHelpers.AddQueryString(address, "sig", signature);
            }

            var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync()).Value<JObject>("data");

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected virtual string ComputeSignature(string address)
        {
            using (var algorithm = new HMACSHA256(Encoding.UTF8.GetBytes(Options.ClientSecret)))
            {
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
