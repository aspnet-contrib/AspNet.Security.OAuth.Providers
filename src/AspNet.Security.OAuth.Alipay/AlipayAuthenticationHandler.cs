using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNet.Security.OAuth.Alipay
{
    /// <summary>
    /// 支付宝 OAUTH2.0 网站登录
    /// 作者：Vicente.Yu
    /// 创建时间：2019-12-12 00:35:35
    /// QQ：8383463
    /// </summary>
    public class AlipayAuthenticationHandler : OAuthHandler<AlipayAuthenticationOptions>
    {
        private const string State = "state";
        public AlipayAuthenticationHandler(
            [NotNull] IOptionsMonitor<AlipayAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            string stateValue = Options.StateDataFormat.Protect(properties);

            var address = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
            {
                ["app_id"] = Options.ClientId,
                ["scope"] = FormatScope(),
                ["redirect_uri"] = redirectUri,
                [State] = stateValue
            });
            return address;
        }
        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var query = Request.Query.ToDictionary(c => c.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);
            if (query.TryGetValue("auth_code", out var auth_code))
            {
                query["code"] = auth_code;
                Request.QueryString = QueryString.Create(query);
            }
            return await base.HandleRemoteAuthenticateAsync();
        }
        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            var sortedparams = new SortedDictionary<string, string>()
            {
                ["app_id"] = Options.ClientId,
                ["method"] = "alipay.system.oauth.token",
                ["format"] = "JSON",
                ["charset"] = "UTF-8",
                ["sign_type"] = "RSA2",
                ["timestamp"] = Clock.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ["version"] = "1.0",
                ["code"] = context.Code,
                ["grant_type"] = "authorization_code",
            };
            var signparames = RSA2Sign(sortedparams);
            if (Options.NotValidateSign)
            {
                signparames.Remove("timestamp");
                signparames.Remove("sign");
            }
            string address = QueryHelpers.AddQueryString(Options.TokenEndpoint, signparames);

            using var request = new HttpRequestMessage(HttpMethod.Get, address);
            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }
            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            using var alipay_system_oauth_token_response = JsonDocument.Parse(payload.RootElement.GetProperty("alipay_system_oauth_token_response").ToString());

            return OAuthTokenResponse.Success(alipay_system_oauth_token_response);
        }
        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            var sortedparams = new SortedDictionary<string, string>()
            {
                ["app_id"] = Options.ClientId,
                ["method"] = "alipay.user.info.share",
                ["format"] = "JSON",
                ["charset"] = "UTF-8",
                ["sign_type"] = "RSA2",
                ["timestamp"] = Clock.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ["version"] = "1.0",
                ["auth_token"] = tokens.AccessToken,
            };
            var signparames = RSA2Sign(sortedparams);
            if (Options.NotValidateSign)
            {
                signparames.Remove("timestamp");
                signparames.Remove("sign");
            }
            string address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, signparames);

            using var response = await Backchannel.GetAsync(address);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }
            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            using var alipay_user_info_share_response = JsonDocument.Parse(payload.RootElement.GetProperty("alipay_user_info_share_response").ToString());

            string status = alipay_user_info_share_response.RootElement.GetString("code");
            if (status != "10000")
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following message: {Message}.",
                                /* Status: */ status,
                                /* Message: */ payload.RootElement.GetString("msg"));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, alipay_user_info_share_response.RootElement.GetString("user_id"), ClaimValueTypes.String, Options.ClaimsIssuer));

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, alipay_user_info_share_response.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected override string FormatScope() => string.Join(",", Options.Scope);
        private IDictionary<string, string> RSA2Sign(SortedDictionary<string, string> source)
        {
            var textparams = source.Where(p => !string.IsNullOrEmpty(p.Value)).Select(p => p.Key + "=" + p.Value);
            if (!textparams.Any())
                throw new HttpRequestException("sign fail");
            var rsa2 = new RSAHelper(RSAType.RSA2, Encoding.UTF8, Options.ClientSecret, Options.AlipayPublicKey);
            source.Add("sign", rsa2.Sign(string.Join("&", textparams)));
            return source;
        }
    }
}
