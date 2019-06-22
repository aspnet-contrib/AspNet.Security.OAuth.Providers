/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            string requestUri = Options.UserInformationEndpoint;
            var fields = Options.Fields
                .Where(f => !string.Equals(f, LinkedInAuthenticationConstants.EmailAddressField, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // If at least one field is specified, append the fields to the endpoint URL.
            if (fields.Count > 0)
            {
                requestUri = QueryHelpers.AddQueryString(requestUri, "projection", $"({string.Join(",", fields)})");
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("x-li-format", "json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            using var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            if (!string.IsNullOrEmpty(Options.EmailAddressEndpoint) &&
                Options.Fields.Contains(LinkedInAuthenticationConstants.EmailAddressField))
            {
                string email = await GetEmailAsync(tokens);
                if (!string.IsNullOrEmpty(email))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, Options.ClaimsIssuer));
                }
            }

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected virtual async Task<string> GetEmailAsync([NotNull] OAuthTokenResponse tokens)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, Options.EmailAddressEndpoint);
            request.Headers.Add("x-li-format", "json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            using var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the email address: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the email address.");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            if (!payload.RootElement.TryGetProperty("elements", out var emails))
            {
                return null;
            }

            return emails
                .EnumerateArray()
                .Select((p) => p.GetProperty("handle~"))
                .Select((p) => p.GetString("emailAddress"))
                .FirstOrDefault();
        }
    }
}
