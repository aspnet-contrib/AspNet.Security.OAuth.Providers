/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Odnoklassniki
{
    public class OdnoklassnikiAuthenticationHandler : OAuthHandler<OdnoklassnikiAuthenticationOptions>
    {
        public OdnoklassnikiAuthenticationHandler(HttpClient client)
            : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);

            // Signing.
            // Call API methods using access_token instead of session_key parameter
            // Calculate every request signature parameter sig using a little bit different way described in
            // https://apiok.ru/dev/methods/
            // session_secret_key = MD5(access_token + application_secret_key)
            // sig = MD5(request_params_composed_string + session_secret_key)
            // Don't include access_token into request_params_composed_string
            var args = new Dictionary<string, string>
            {
                { "application_key", Options.ClientPublic },
                { "__online", "false" }
            };

            if (Options.Fields.Count != 0)
            {
                args.Add("fields", string.Join(",", Options.Fields));
            }

            var signature = string.Concat(args.OrderBy(x => x.Key).Select(x => $"{x.Key}={x.Value}"));
            signature = (signature + (tokens.AccessToken + Options.ClientSecret).GetMd5Hash()).GetMd5Hash();

            args.Add("sig", signature);

            address = QueryHelpers.AddQueryString(address, args);

            HttpResponseMessage response = await Backchannel.GetAsync(address, Context.RequestAborted);
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

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), properties, Options.AuthenticationScheme);
            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, OdnoklassnikiAuthenticationHelper.GetId(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, OdnoklassnikiAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, OdnoklassnikiAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, OdnoklassnikiAuthenticationHelper.GetFirstName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, OdnoklassnikiAuthenticationHelper.GetLastName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:odnoklassniki:link", OdnoklassnikiAuthenticationHelper.GetLink(payload), Options.ClaimsIssuer);

            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected override string FormatScope()
        {
            return string.Join(",", Options.Scope);
        }
    }
}
