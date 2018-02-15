/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using static AspNet.Security.OAuth.Strava.StravaAuthenticationConstants;

namespace AspNet.Security.OAuth.Strava
{
    public class StravaAuthenticationHandler : OAuthHandler<StravaAuthenticationOptions>
    {
        public StravaAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

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

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, StravaAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, StravaAuthenticationHelper.GetUsername(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, StravaAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, StravaAuthenticationHelper.GetFirstName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, StravaAuthenticationHelper.GetLastName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.StateOrProvince, StravaAuthenticationHelper.GetState(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Country, StravaAuthenticationHelper.GetCountry(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Gender, StravaAuthenticationHelper.GetGender(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.City, StravaAuthenticationHelper.GetCity(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.Profile, StravaAuthenticationHelper.GetProfile(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.ProfileMedium, StravaAuthenticationHelper.GetProfileMedium(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.CreatedAt, StravaAuthenticationHelper.GetCreationDate(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.UpdatedAt, StravaAuthenticationHelper.GetModificationDate(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(Claims.Premium, StravaAuthenticationHelper.GetPremium(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected override string FormatScope() => string.Join(",", Options.Scope);
    }
}
