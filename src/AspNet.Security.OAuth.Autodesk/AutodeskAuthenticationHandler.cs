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
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Autodesk
{
    public class AutodeskAuthenticationHandler : OAuthHandler<AutodeskAuthenticationOptions>
    {
        public AutodeskAuthenticationHandler([NotNull] HttpClient client)
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
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, AutodeskAuthenticationHelper.GetUserId(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, AutodeskAuthenticationHelper.GetUserName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, AutodeskAuthenticationHelper.GetFirstName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, AutodeskAuthenticationHelper.GetLastName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, AutodeskAuthenticationHelper.GetEmailAddress(payload), ClaimValueTypes.Email, Options.ClaimsIssuer)
                    .AddOptionalClaim(AutodeskAuthenticationConstants.Claims.EmailVerified, AutodeskAuthenticationHelper.GetEmailVerified(payload), ClaimValueTypes.Boolean, Options.ClaimsIssuer)
                    .AddOptionalClaim(AutodeskAuthenticationConstants.Claims.TwoFactorEnabled, AutodeskAuthenticationHelper.GetTwoFactorEnabled(payload), ClaimValueTypes.Boolean, Options.ClaimsIssuer);
            
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}
