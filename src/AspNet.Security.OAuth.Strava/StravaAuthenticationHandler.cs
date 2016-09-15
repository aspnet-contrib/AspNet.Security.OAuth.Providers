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

namespace AspNet.Security.OAuth.Strava
{
	/// <summary>
	/// The Strava Authentication Handler
	/// </summary>
	/// <seealso cref="OAuthHandler{OAuthOptions}" />
	public class StravaAuthenticationHandler : OAuthHandler<StravaAuthenticationOptions>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="StravaAuthenticationHandler"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		public StravaAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

		/// <summary>
		/// Creates the ticket.
		/// </summary>
		/// <param name="identity">The identity.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="tokens">The tokens.</param>
		/// <returns></returns>
		protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, StravaAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                 .AddOptionalClaim(ClaimTypes.Name, StravaAuthenticationHelper.GetUsername(payload), Options.ClaimsIssuer)
                 .AddOptionalClaim(ClaimTypes.Email, StravaAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}
