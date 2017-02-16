using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.JetbBainsHub {
    internal class JetBrainsHubHandler : OAuthHandler<JetBrainsHubOptions> {
        public JetBrainsHubHandler(HttpClient httpClient)
            : base(httpClient) { }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity,
            AuthenticationProperties properties,
            OAuthTokenResponse tokens) {
            // Get the JetBrains Hub user
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"An error occured when retrieving user information ({response.StatusCode}). Please check if the authentication information is correct.");

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);
            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);

            var identifier = JetBrainsHubHelper.GetId(payload);
            if (!string.IsNullOrWhiteSpace(identifier))
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identifier, ClaimValueTypes.String, Options.ClaimsIssuer));

            var name = JetBrainsHubHelper.GetName(payload);
            if (!string.IsNullOrWhiteSpace(name))
                identity.AddClaim(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, Options.ClaimsIssuer));

            var email = JetBrainsHubHelper.GetEmail(payload);
            if (!string.IsNullOrWhiteSpace(email))
                identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, Options.ClaimsIssuer));

            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri) {
            var queryStrings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                {"response_type", "code"},
                {"client_id", Options.ClientId},
                {"redirect_url", redirectUri},
                {"request_credentials", "default"}
            };

            AddQueryString(queryStrings, properties, "scope", FormatScope());
            AddQueryString(queryStrings, properties, "access_type", Options.AccessType.ToString());

            var state = Options.StateDataFormat.Protect(properties);
            queryStrings.Add("state", state);

            var authorizationEndpoint = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, queryStrings);
            return authorizationEndpoint;
        }

        private static void AddQueryString(
            IDictionary<string, string> queryStrings,
            AuthenticationProperties properties,
            string name,
            string defaultValue = null) {
            string value;
            if (!properties.Items.TryGetValue(name, out value)) {
                value = defaultValue;
            }
            else {
                // Remove the parameter from AuthenticationProperties so it won't be serialized to state parameter
                properties.Items.Remove(name);
            }

            if (value == null) {
                return;
            }

            queryStrings[name] = value;
        }
    }
}