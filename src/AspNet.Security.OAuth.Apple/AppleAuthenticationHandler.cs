/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// Defines a handler for authentication using Apple.
    /// </summary>
    public class AppleAuthenticationHandler : OAuthHandler<AppleAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppleAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The authentication options.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="encoder">The URL encoder to use.</param>
        /// <param name="clock">The system clock to use.</param>
        public AppleAuthenticationHandler(
            [NotNull] IOptionsMonitor<AppleAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            //// TODO No user information endpoint is documented yet.
            //string endpoint = Options.UserInformationEndpoint;
            //
            //// TODO Append any additional query string parameters required
            ////endpoint = QueryHelpers.AddQueryString(endpoint, "token", tokens.AccessToken);
            //
            //var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //
            //// TODO Add any HTTP request headers required
            ////request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
            //
            //var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            //if (!response.IsSuccessStatusCode)
            //{
            //    Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
            //                    "returned a {Status} response with the following payload: {Headers} {Body}.",
            //                    /* Status: */ response.StatusCode,
            //                    /* Headers: */ response.Headers.ToString(),
            //                    /* Body: */ await response.Content.ReadAsStringAsync());
            //
            //    throw new HttpRequestException("An error occurred while retrieving the user profile from Apple.");
            //}
            //
            //var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            // See https://developer.okta.com/blog/2019/06/04/what-the-heck-is-sign-in-with-apple
            string encodedIdToken = tokens.Response.Value<string>("id_token");
            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(encodedIdToken));

            // TODO Proper validation and error case handling
            int endOfTokenHeader = decodedToken.IndexOf('}');
            string claimsString = decodedToken.Substring(endOfTokenHeader + 1);

            var claims = JObject.Parse(claimsString);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, claims.Value<string>("sub")));

            var principal = new ClaimsPrincipal(identity);

            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
    }
}
