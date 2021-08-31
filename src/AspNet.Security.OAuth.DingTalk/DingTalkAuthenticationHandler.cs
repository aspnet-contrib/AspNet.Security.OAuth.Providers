/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
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
    public partial class DingTalkAuthenticationHandler : OAuthHandler<DingTalkAuthenticationOptions>
    {
        public DingTalkAuthenticationHandler(
            [NotNull] IOptionsMonitor<DingTalkAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
        {
            string stateValue = Options.StateDataFormat.Protect(properties);
            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string?>
            {
                ["appid"] = Options.ClientId,
                ["response_type"] = "code",
                ["scope"] = FormatScope(),
                ["redirect_uri"] = redirectUri,
                ["state"] = stateValue
            });
        }

        // See https://developers.dingtalk.com/document/app/obtain-orgapp-token for details.
        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, new Dictionary<string, string?>
            {
                ["appkey"] = Options.ClientId,
                ["appsecret"] = Options.ClientSecret
            });
            using var response = await Backchannel.GetAsync(address);
            using var payload = await GetPayloadAsync(response);

            return OAuthTokenResponse.Success(payload);
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            var code = Request.Query["code"];
            var userInfoTmp = await GetUserInfoTemporaryAsync(code);
            var userIdentifier = await GetUserIdentifierAsync(userInfoTmp.unionid!, tokens.AccessToken);

            // See https://developers.dingtalk.com/document/app/query-user-details for details.
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);

            using var requestContent = new ReadOnlyMemoryContent(JsonSerializer.SerializeToUtf8Bytes(new
            {
                userid = userIdentifier.userid,
                language = Options.Language
            }));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            using var response = await Backchannel.PostAsync(address, requestContent, Context.RequestAborted);
            using var payload = await GetPayloadAsync(response);

            var result = payload.RootElement.GetProperty("result");

            using var userInfo = JsonDocument.Parse(await MergePayloadAsync(result, userInfoTmp));
            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, userInfo.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }

        protected override string FormatScope() => string.Join(',', Options.Scope);

        private static async Task<System.ReadOnlyMemory<byte>> MergePayloadAsync(JsonElement result, (string? nick, string? unionid, string? openid, bool mainOrgAuthHighLevel) userInfoTmp)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using var writer = new Utf8JsonWriter(bufferWriter);
            writer.WriteStartObject();
            writer.WriteString("nick", userInfoTmp.nick);
            writer.WriteString("openid", userInfoTmp.openid);
            writer.WriteBoolean("main_org_auth_high_level", userInfoTmp.mainOrgAuthHighLevel);
            foreach (var item in result.EnumerateObject())
            {
                item.WriteTo(writer);
            }

            writer.WriteEndObject();
            await writer.FlushAsync();
            return bufferWriter.WrittenMemory;
        }

        private async Task<JsonDocument> GetPayloadAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var content = await response.Content.ReadAsStringAsync(Context.RequestAborted);
            var payload = JsonDocument.Parse(content);

            int errorCode = payload.RootElement.GetProperty("errcode").GetInt32();
            if (errorCode != 0)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {ErrorCode} response with the following message: {Message}.",
                                /* Status: */ errorCode,
                                /* Message: */  payload.RootElement.GetString("errmsg"));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            return payload;
        }

        private static string Signature(string timestamp, string secret)
        {
            var encoding = System.Text.Encoding.UTF8;
            byte[] keyByte = string.IsNullOrEmpty(secret) ? Array.Empty<byte>() : encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(timestamp);

            using (var hmac = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmac.ComputeHash(messageBytes);
                return System.Convert.ToBase64String(hashMessage);
            }
        }

        // See https://developers.dingtalk.com/document/app/obtain-the-user-information-based-on-the-sns-temporary-authorization for details.
        private async Task<(string? nick, string? unionid, string? openid, bool mainOrgAuthHighLevel)> GetUserInfoTemporaryAsync(string code)
        {
            var timestamp = (Clock.UtcNow - System.DateTimeOffset.UnixEpoch).TotalMilliseconds.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);

            var address = QueryHelpers.AddQueryString(Options.GetUserInfoByCodeEndpoint, new Dictionary<string, string?>
            {
                ["accessKey"] = Options.ClientId,
                ["timestamp"] = timestamp,
                ["signature"] = Signature(timestamp, Options.ClientSecret),
            });
            using var requestContent = new ReadOnlyMemoryContent(JsonSerializer.SerializeToUtf8Bytes(new
            {
                tmp_auth_code = code
            }));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            using var response = await Backchannel.PostAsync(address, requestContent, Context.RequestAborted);
            using var payload = await GetPayloadAsync(response);

            var userInfo = payload.RootElement.GetProperty("user_info");
            return (nick: userInfo.GetString("nick"), unionid: userInfo.GetString("unionid"), openid: userInfo.GetString("openid"), mainOrgAuthHighLevel: userInfo.GetProperty("main_org_auth_high_level").GetBoolean());
        }

        // See https://developers.dingtalk.com/document/app/query-a-user-by-the-union-id for details.
        private async Task<(string? userid, int contactType)> GetUserIdentifierAsync(string unionid, string accessToken)
        {
            var address = QueryHelpers.AddQueryString(Options.GetByUnionIdEndpoint, "access_token", accessToken);
            using var requestContent = new ReadOnlyMemoryContent(JsonSerializer.SerializeToUtf8Bytes(new
            {
                unionid = unionid
            }));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            using var response = await Backchannel.PostAsync(address, requestContent);
            using var payload = await GetPayloadAsync(response);

            var result = payload.RootElement.GetProperty("result");
            return (userid: result.GetString("userid"), contactType: result.GetProperty("contact_type").GetInt32());
        }
    }
}
