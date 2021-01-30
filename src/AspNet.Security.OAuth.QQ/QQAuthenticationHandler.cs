/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.QQ
{
    public class QQAuthenticationHandler : OAuthHandler<QQAuthenticationOptions>
    {
        public QQAuthenticationHandler(
            [NotNull] IOptionsMonitor<QQAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            (int errorCode, string? openId, string? unionId) = await GetUserIdentifierAsync(tokens);

            if (errorCode != 0 || string.IsNullOrEmpty(openId))
            {
                throw new HttpRequestException($"An error (Code:{errorCode}) occurred while retrieving the user identifier.");
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, openId, ClaimValueTypes.String, Options.ClaimsIssuer));

            if (!string.IsNullOrEmpty(unionId))
            {
                identity.AddClaim(new Claim(QQAuthenticationConstants.Claims.UnionId, unionId, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            string address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string?>(3)
            {
                ["oauth_consumer_key"] = Options.ClientId,
                ["access_token"] = tokens.AccessToken,
                ["openid"] = openId,
            });

            using var response = await Backchannel.GetAsync(address);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
            using var payload = JsonDocument.Parse(stream);

            int status = payload.RootElement.GetProperty("ret").GetInt32();
            if (status != 0)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following message: {Message}.",
                                /* Status: */ status,
                                /* Message: */ payload.RootElement.GetString("msg"));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            // See https://wiki.connect.qq.com/%E4%BD%BF%E7%94%A8authorization_code%E8%8E%B7%E5%8F%96access_token for details
            string address = QueryHelpers.AddQueryString(Options.TokenEndpoint, new Dictionary<string, string?>(6)
            {
                ["client_id"] = Options.ClientId,
                ["client_secret"] = Options.ClientSecret,
                ["redirect_uri"] = context.RedirectUri,
                ["code"] = context.Code,
                ["grant_type"] = "authorization_code",
                ["fmt"] = "json" // Return JSON instead of x-www-form-urlencoded which is default due to historical reasons
            });

            using var request = new HttpRequestMessage(HttpMethod.Get, address);

            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
            var payload = JsonDocument.Parse(stream);

            return OAuthTokenResponse.Success(payload);
        }

        private async Task<(int errorCode, string? openId, string? unionId)> GetUserIdentifierAsync(OAuthTokenResponse tokens)
        {
            // See https://wiki.connect.qq.com/unionid%E4%BB%8B%E7%BB%8D for details
            var queryString = new Dictionary<string, string?>(3)
            {
                ["access_token"] = tokens.AccessToken,
                ["fmt"] = "json" // Return JSON instead of JSONP which is default due to historical reasons
            };

            if (Options.ApplyForUnionId)
            {
                queryString.Add("unionid", "1");
            }

            string address = QueryHelpers.AddQueryString(Options.UserIdentificationEndpoint, queryString);
            using var request = new HttpRequestMessage(HttpMethod.Get, address);

            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user identifier: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving the user identifier.");
            }

            using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
            using JsonDocument payload = JsonDocument.Parse(stream);

            var payloadRoot = payload.RootElement;

            int errorCode =
                payloadRoot.TryGetProperty("error", out var errorCodeElement) && errorCodeElement.ValueKind == JsonValueKind.Number ?
                errorCodeElement.GetInt32() :
                0;

            return (errorCode, openId: payloadRoot.GetString("openid"), unionId: payloadRoot.GetString("unionid"));
        }

        protected override string FormatScope() => FormatScope(Options.Scope);

        protected override string FormatScope([NotNull] IEnumerable<string> scopes) => string.Join(',', scopes);
    }
}
