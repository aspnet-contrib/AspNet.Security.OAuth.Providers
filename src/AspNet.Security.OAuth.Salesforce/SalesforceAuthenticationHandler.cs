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
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Salesforce
{
    public class SalesforceAuthenticationHandler : OAuthHandler<SalesforceAuthenticationOptions>
    {
        public SalesforceAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var identityServiceUrl = tokens.Response.Value<string>("id");
            var salesforceInstanceUrl = tokens.Response.Value<string>("instance_url");

            var address = QueryHelpers.AddQueryString(identityServiceUrl, "token", tokens.AccessToken);
            var request = new HttpRequestMessage(HttpMethod.Get, address);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred when retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred when retrieving the user from the Salesforce identity service.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            Logger.LogTrace(payload.ToString());
            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, SalesforceAuthenticationHelper.GetUserIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, SalesforceAuthenticationHelper.GetUserName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:instance_url", salesforceInstanceUrl, Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:organization_id", payload.Value<string>("organization_id"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:username", payload.Value<string>("username"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:nick_name", payload.Value<string>("nick_name"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:display_name", payload.Value<string>("display_name"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:email", payload.Value<string>("email"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:email_verified", payload.Value<string>("email_verified"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:first_name", payload.Value<string>("first_name"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:last_name", payload.Value<string>("last_name"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:photos.picture", payload["photos"].Value<string>("picture"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:photos.thumbnail", payload["photos"].Value<string>("thumbnail"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:user_type", payload.Value<string>("user_type"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:language", payload.Value<string>("language"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:locale", payload.Value<string>("locale"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:utcOffset", payload.Value<int>("utcOffset").ToString(), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:last_modified_date", payload.Value<string>("last_modified_date"), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:salesforce:is_app_installed", payload.Value<bool>("is_app_installed").ToString(), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}
