/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Yammer
{
    public class YammerAuthenticationHandler : OAuthHandler<YammerAuthenticationOptions>
    {
        public YammerAuthenticationHandler(
            [NotNull] IOptionsMonitor<YammerAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
            {
                var principal = new ClaimsPrincipal(identity);
                var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
                context.RunClaimActions();

                await Options.Events.CreatingTicket(context);

                return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
            }
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] string code, [NotNull] string redirectUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = Options.ClientId,
                ["redirect_uri"] = redirectUri,
                ["client_secret"] = Options.ClientSecret,
                ["code"] = code,
                ["grant_type"] = "authorization_code"
            });

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            // Note: Yammer doesn't return a standard OAuth2 response. To make this middleware compatible
            // with the OAuth2 generic middleware, a compliant JSON payload is generated manually.
            // See https://developer.yammer.com/docs/oauth-2 for more information about this process.
            string accessToken;

            using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
            {
                accessToken = payload.RootElement.GetProperty("access_token").GetString("token");
            }

            // HACK Work out the best way to do this with System.Text.Json
            using (var stream = new MemoryStream())
            {
                WriteAccessToken(accessToken, stream);

                var copy = JsonDocument.Parse(stream);
                return OAuthTokenResponse.Success(copy);
            }
        }

        private void WriteAccessToken(string token, Stream stream)
        {
            var output = new StreamPipeWriter(stream);
            var writer = new Utf8JsonWriter(output);

            writer.WriteStartObject();
            writer.WriteString("access_token", token);
            writer.WriteString("token_type", string.Empty);
            writer.WriteString("refresh_token", string.Empty);
            writer.WriteString("expires_in", string.Empty);
            writer.WriteEndObject();

            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
        }
    }
}
