/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net.Http;
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

namespace AspNet.Security.OAuth.Vkontakte
{
    public class VkontakteAuthenticationHandler : OAuthHandler<VkontakteAuthenticationOptions>
    {
        public VkontakteAuthenticationHandler(HttpClient client)
            : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity, [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);

            if (Options.Fields.Count != 0)
            {
                address = QueryHelpers.AddQueryString(address, "fields", string.Join(",", Options.Fields));
            }

            var response = await Backchannel.GetAsync(address, Context.RequestAborted);
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
            var user = (JObject) payload["response"][0];

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), properties, Options.AuthenticationScheme);
            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, user);

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, VkontakteAuthenticationHelper.GetId(user), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, VkontakteAuthenticationHelper.GetFirstName(user), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, VkontakteAuthenticationHelper.GetLastName(user), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Hash, VkontakteAuthenticationHelper.GetHash(user), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:vkontakte:photo:link", VkontakteAuthenticationHelper.GetPhoto(user), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:vkontakte:photo_thumb:link", VkontakteAuthenticationHelper.GetPhotoThumbnail(user), Options.ClaimsIssuer);

            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }
    }
}