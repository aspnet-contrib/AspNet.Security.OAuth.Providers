using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using AspNet.Security.OAuth.Extensions;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Weibo
{
    public class WeiboAuthenticationHandler : OAuthHandler<WeiboAuthenticationOptions>
    {
        public WeiboAuthenticationHandler(HttpClient client) : base(client) { }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var queryString = new Dictionary<string, string>()
            {
                {"access_token",tokens.AccessToken },
                {"uid",tokens.Response.Value<string>("uid") }
            };
            var endpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, queryString);
            var response = await Backchannel.GetAsync(endpoint, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError($"An error occurred while retrieving the user information: the remote server returned a " +
                    $"{response.StatusCode} response with the following payload: {await response.Content.ReadAsStringAsync()}.");

                throw new HttpRequestException("An error occurred when retrieving user information.");
            }
            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, WeiboAuthenticationHelper.GetId(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, WeiboAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weibo:screen_name", WeiboAuthenticationHelper.GetScreenName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weibo:profile_image_url", WeiboAuthenticationHelper.GetProfileImageUrl(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Gender, WeiboAuthenticationHelper.GetGender(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weibo:avatar_large", WeiboAuthenticationHelper.GetAvatarLarge(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weibo:avatar_hd", WeiboAuthenticationHelper.GetAvatarHD(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weibo:cover_image_phone", WeiboAuthenticationHelper.GetCoverImagePhone(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:weibo:location", WeiboAuthenticationHelper.GetLocation(payload), Options.ClaimsIssuer);            

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected override string FormatScope()
        {
            return string.Join(",", Options.Scope);
        }
    }
}
