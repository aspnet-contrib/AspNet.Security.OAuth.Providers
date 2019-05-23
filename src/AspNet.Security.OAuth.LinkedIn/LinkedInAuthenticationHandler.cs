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
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
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
            string address = Options.UserInformationEndpoint;
            var fields = Options.Fields
                .Where(f => !string.Equals(f, LinkedInAuthenticationConstants.EmailAddressField, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // If at least one field is specified,
            // append the fields to the endpoint URL.
            if (fields.Count != 0)
            {
                address = QueryHelpers.AddQueryString(address, new Dictionary<string, string>
                {
                    ["projection"] = $"({string.Join(",", fields)})",
                });
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
                payload.Last.AddAfterSelf(new JProperty("emailAddress", await GetEmailAsync(tokens)));
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected virtual async Task<string> GetEmailAsync([NotNull] OAuthTokenResponse tokens)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.EmailAddressEndpoint);
            request.Headers.Add("x-li-format", "json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the email address: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the email address.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            return (from address in payload.Value<JArray>("elements")
                    select address.Value<JObject>("handle~")?.Value<string>("emailAddress")).FirstOrDefault();
        }
    }
}
