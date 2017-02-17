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

namespace AspNet.Security.OAuth.LinkedIn
{
    public class LinkedInAuthenticationHandler : OAuthHandler<LinkedInAuthenticationOptions>
    {
        public LinkedInAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var address = Options.UserInformationEndpoint;

            // If at least one field is specified,
            // append the fields to the endpoint URL.
            if (Options.Fields.Count != 0)
            {
                address = address.Insert(address.LastIndexOf("~") + 1, $":({ string.Join(",", Options.Fields)})");
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

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, LinkedInAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, LinkedInAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, LinkedInAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, LinkedInAuthenticationHelper.GetGivenName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, LinkedInAuthenticationHelper.GetFamilyName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:profile", LinkedInAuthenticationHelper.GetPublicProfileUrl(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:profilepicture", LinkedInAuthenticationHelper.GetProfilePictureUrl(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:industry", LinkedInAuthenticationHelper.GetIndustry(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:summary", LinkedInAuthenticationHelper.GetSummary(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:headline", LinkedInAuthenticationHelper.GetHeadline(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:positions", LinkedInAuthenticationHelper.GetPositions(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:maidenname", LinkedInAuthenticationHelper.GetMaidenName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:phoneticfirstname", LinkedInAuthenticationHelper.GetPhoneticFirstName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:phoneticlastname", LinkedInAuthenticationHelper.GetPhoneticLastName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:phoneticname", LinkedInAuthenticationHelper.GetPhoneticName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:location", LinkedInAuthenticationHelper.GetLocation(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:specialties", LinkedInAuthenticationHelper.GetSpecialties(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:numconnections", LinkedInAuthenticationHelper.GetNumConnections(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:numconnectionscapped", LinkedInAuthenticationHelper.GetNumConnectionsCapped(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:currentshare", LinkedInAuthenticationHelper.GetCurrentShare(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:sitestandardprofilerequest", LinkedInAuthenticationHelper.GetSiteStandardProfileRequest(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:apistandardprofilerequest", LinkedInAuthenticationHelper.GetApiStandardProfileRequest(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:linkedin:pictureurls", LinkedInAuthenticationHelper.GetPictureUrls(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}