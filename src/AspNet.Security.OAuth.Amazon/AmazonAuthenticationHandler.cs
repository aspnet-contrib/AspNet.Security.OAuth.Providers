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
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Amazon
{
    /// <summary>
    /// A class representing an authentication handler for Amazon.
    /// </summary>
    public class AmazonAuthenticationHandler : OAuthHandler<AmazonAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> to use.</param>
        public AmazonAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, 
            [NotNull] OAuthTokenResponse tokens)
        {
            var endpoint = Options.UserInformationEndpoint;

            if (Options.Fields.Count > 0)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "fields", string.Join(",", Options.Fields));
            }

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            identity
                .AddOptionalClaim(ClaimTypes.NameIdentifier, AmazonAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Email, AmazonAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.Name, AmazonAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                .AddOptionalClaim(ClaimTypes.PostalCode, AmazonAuthenticationHelper.GetPostalCode(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}
