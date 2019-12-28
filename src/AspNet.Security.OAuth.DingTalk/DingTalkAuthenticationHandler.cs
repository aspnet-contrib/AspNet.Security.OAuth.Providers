/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

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
            using var payload = JsonDocument.Parse("{\"access_token\":\"" + context.Code + "\"}");
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
            using var response = await Backchannel.PostAsJsonAsync(address, new { tmp_auth_code = tokens.AccessToken });
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
            //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1141CA123A8EF446A2C71DEC1BA9E2C1", ClaimValueTypes.String, Options.ClaimsIssuer));
            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, userInfo);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected override string FormatScope() => string.Join(",", Options.Scope);


        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private string GetTimeStamp()
        {
            var ts = System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            return System.Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
        private string Signature(string timestamp, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(timestamp);
            using (var hmacsha256 = new System.Security.Cryptography.HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return System.Convert.ToBase64String(hashmessage);
            }
        }
        //private async Task<string> GetUserTokenAsync(OAuthTokenResponse tokens)
        //{
        //    var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, new Dictionary<string, string>()
        //    {
        //        ["appid"] = Options.ClientId,
        //        ["appsecret"] = Options.ClientSecret
        //    });

        //    using var request = new HttpRequestMessage(HttpMethod.Get, address);

        //    using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        Logger.LogError("An error occurred while retrieving an access token: the remote server " +
        //                        "returned a {Status} response with the following payload: {Headers} {Body}.",
        //                        /* Status: */ response.StatusCode,
        //                        /* Headers: */ response.Headers.ToString(),
        //                        /* Body: */ await response.Content.ReadAsStringAsync());

        //        return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
        //    }

        //}
    }
}

