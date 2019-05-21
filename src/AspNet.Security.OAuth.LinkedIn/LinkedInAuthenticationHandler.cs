/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.LinkedIn
{
    public class LinkedInAuthenticationHandler : OAuthHandler<LinkedInAuthenticationOptions>
    {
        public LinkedInAuthenticationHandler(
            [NotNull] IOptionsMonitor<LinkedInAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var address = Options.UserInformationEndpoint;
            var fields = Options.Fields.Where(f => f != LinkedInAuthenticationConstants.EmailAddressField).ToList();

            // If at least one field is specified,
            // append the fields to the endpoint URL.
            if (fields.Count != 0)
            {
                address = $"{address}?projection=({string.Join(",", fields)})";
            }

            var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Add("x-li-format", "json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            if (Options.Fields.Contains(LinkedInAuthenticationConstants.EmailAddressField))
            {
                var emailAddressRequest = new HttpRequestMessage(HttpMethod.Get, Options.EmailAddressEndpoint);
                emailAddressRequest.Headers.Add("x-li-format", "json");
                emailAddressRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

                var emailAddressResponse = await Backchannel.SendAsync(emailAddressRequest, Context.RequestAborted);
                if (!emailAddressResponse.IsSuccessStatusCode)
                {
                    Logger.LogError("An error occurred while retrieving the email address: the remote server " +
                                    "returned a {Status} response with the following payload: {Headers} {Body}.",
                                    /* Status: */ emailAddressResponse.StatusCode,
                                    /* Headers: */ emailAddressResponse.Headers.ToString(),
                                    /* Body: */ await emailAddressResponse.Content.ReadAsStringAsync());

                    throw new HttpRequestException("An error occurred while retrieving the email address.");
                }

                var emailAddressPayload = JObject.Parse(await emailAddressResponse.Content.ReadAsStringAsync());
                var emailAddress = emailAddressPayload.SelectToken("elements[0].handle~.emailAddress").ToString();

                var emailProperty = new JProperty("emailAddress", emailAddress);
                payload.Last.AddAfterSelf(emailProperty);
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
    }
}
