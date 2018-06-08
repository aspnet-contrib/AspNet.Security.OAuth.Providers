/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Odnoklassniki
{
    public class OdnoklassnikiAuthenticationHandler : OAuthHandler<OdnoklassnikiAuthenticationOptions>
    {
        public OdnoklassnikiAuthenticationHandler(
            [NotNull] IOptionsMonitor<OdnoklassnikiAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var md5Inner = GetMd5(tokens.AccessToken + Options.ClientSecret);
            var parametersForSig = PrepareParametersForSig(md5Inner);
            var sig = GetMd5(parametersForSig);
            var address = PrepareUserInfoAddress(tokens.AccessToken, sig);

            var response = await Backchannel.GetAsync(address, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                await LogErrorFromResponseAsync(response);

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var principal = new ClaimsPrincipal(identity);

            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);

            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        private static string GetMd5([NotNull] string value)
        {
            var md5 = MD5.Create();

            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            var stringBuilder = new StringBuilder();

            foreach (var b in bytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        private string PrepareParametersForSig([NotNull] string md5Inner)
        {
            return $"application_key={Options.ApplicationKey}" +
                   "format=json" +
                   "method=users.getCurrentUser" +
                   $"{md5Inner}";
        }

        private string PrepareUserInfoAddress([NotNull] string accessToken, [NotNull] string sig)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "application_key", Options.ApplicationKey);
            address = QueryHelpers.AddQueryString(address, "format", "json");
            address = QueryHelpers.AddQueryString(address, "method", "users.getCurrentUser");
            address = QueryHelpers.AddQueryString(address, "sig", sig);
            address = QueryHelpers.AddQueryString(address, "access_token", accessToken);

            return address;
        }

        private async Task LogErrorFromResponseAsync([NotNull] HttpResponseMessage response)
        {
            const string error = "An error occurred while retrieving the user profile: the remote server " +
                                 "returned a {Status} response with the following payload: {Headers} {Body}.";

            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Logger.LogError(error, response.StatusCode, response.Headers.ToString(), body);
        }
    }
}
