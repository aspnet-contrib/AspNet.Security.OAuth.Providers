/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Buffers;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.DingTalk
{
    public class DingTalkAuthenticationHandler : OAuthHandler<DingTalkAuthenticationOptions>
    {
        public DingTalkAuthenticationHandler(
            [NotNull] IOptionsMonitor<DingTalkAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            string stateValue = Options.StateDataFormat.Protect(properties);
            var address = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
            {
                ["appid"] = Options.ClientId,
                ["response_type"] = "code",
                ["scope"] = FormatScope(),
                ["redirect_uri"] = redirectUri,
                ["state"] = stateValue
            });
            return address;
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            using var payload = JsonDocument.Parse(await CopyPayloadAsync("access_token", context.Code));
            return await Task.FromResult(OAuthTokenResponse.Success(payload));
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            var timestamp = GetTimeStamp();

            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string>
            {
                ["accessKey"] = Options.ClientId,
                ["timestamp"] = timestamp,
                ["signature"] = Signature(timestamp, Options.ClientSecret),
            });

            using var request = new HttpRequestMessage(HttpMethod.Post, address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            using var requestContent = new ReadOnlyMemoryContent(await CopyPayloadAsync("tmp_auth_code", tokens.AccessToken));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var content = await response.Content.ReadAsStringAsync();
            using var payload = JsonDocument.Parse(content);

            int status = payload.RootElement.GetProperty("errcode").GetInt32();
            if (status != 0)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following message: {Message}.",
                                /* Status: */ status,
                                /* Message: */  payload.RootElement.GetString("errmsg"));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var userInfo = payload.RootElement.GetProperty("user_info");
            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, userInfo);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected override string FormatScope() => string.Join(",", Options.Scope);

        private static async Task<System.ReadOnlyMemory<byte>> CopyPayloadAsync(string propertyName, string value)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();

            using var writer = new Utf8JsonWriter(bufferWriter);

            writer.WriteStartObject();

            writer.WriteString(propertyName, value);

            writer.WriteEndObject();
            await writer.FlushAsync();

            return bufferWriter.WrittenMemory;
        }

        private string GetTimeStamp()
        {
            var ts = Clock.UtcNow - System.DateTimeOffset.UnixEpoch;
            return ts.TotalMilliseconds.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);
        }

        private static string Signature(string timestamp, string secret)
        {
            secret = secret ?? string.Empty;
            var encoding = System.Text.Encoding.UTF8;
            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(timestamp);
            using (var hmac = HMAC.Create("HMACSHA256"))
            {
                hmac.Key = keyBytes;
                byte[] hashMessage = hmac.ComputeHash(messageBytes);
                return System.Convert.ToBase64String(hashMessage);
            }
        }
    }
}
