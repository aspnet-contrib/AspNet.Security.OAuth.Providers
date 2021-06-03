/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Notion
{
    public class NotionAuthenticationHandler : OAuthHandler<NotionAuthenticationOptions>
    {
        public NotionAuthenticationHandler(
            [NotNull] IOptionsMonitor<NotionAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                ["client_id"] = Options.ClientId,
                ["redirect_uri"] = context.RedirectUri,
                ["client_secret"] = Options.ClientSecret,
                ["code"] = context.Code,
                ["grant_type"] = "authorization_code",
            };

            // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
            if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier) &&
                !string.IsNullOrEmpty(codeVerifier))
            {
                tokenRequestParameters[OAuthConstants.CodeVerifierKey] = codeVerifier;
                context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
            }

            using var requestContent = new FormUrlEncodedContent(tokenRequestParameters!);

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            byte[] byteArray = Encoding.ASCII.GetBytes(Options.ClientId + ":" + Options.ClientSecret);
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            requestMessage.Content = requestContent;
            requestMessage.Version = Backchannel.DefaultRequestVersion;

            using var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
            if (response.IsSuccessStatusCode)
            {
                var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                return OAuthTokenResponse.Success(payload);
            }

            var error = "OAuth token endpoint failure: " + await DisplayAsync(response);
            return OAuthTokenResponse.Failed(new Exception(error));
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(
                principal,
                properties,
                Context,
                Scheme,
                Options,
                Backchannel,
                tokens,
                tokens!.Response!.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }

        private static async Task<string> DisplayAsync(HttpResponseMessage response)
        {
            var output = new StringBuilder();
            output.Append("Status: ").Append(response.StatusCode).Append(';');
            output.Append("Headers: ").Append(response.Headers).Append(';');
            output.Append("Body: ").Append(await response.Content.ReadAsStringAsync()).Append(';');
            return output.ToString();
        }
    }
}
