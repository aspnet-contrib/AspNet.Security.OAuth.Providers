/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.WorkWeixin
{
    public class WorkWeixinAuthenticationHandler : OAuthHandler<WorkWeixinAuthenticationOptions>
    {
        public WorkWeixinAuthenticationHandler(
        [NotNull] IOptionsMonitor<WorkWeixinAuthenticationOptions> options,
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
            (int errCode, string? userId) = await GetUserIdentityfierAsync(tokens);
            if (errCode != 0 || string.IsNullOrEmpty(userId))
            {
                throw new HttpRequestException($"An error (Code:{errCode}) occurred while retrieving the user identitfier");
            }

            string address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string?>
            {
                ["access_token"] = tokens.AccessToken,
                ["userid"] = userId,
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

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            errCode = payload.RootElement.GetProperty("errcode").GetInt32();
            if (errCode != 0)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                          "returned a {Status} response with the following message: {Message} .",
                          /* Status: */ errCode,
                          /* Message: */ payload.RootElement.GetString("errmsg"));

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
            string address = QueryHelpers.AddQueryString(Options.TokenEndpoint, new Dictionary<string, string?>()
            {
                ["corpid"] = Options.ClientId,
                ["corpsecret"] = Options.ClientSecret,
            });

            using var response = await Backchannel.GetAsync(address);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            return OAuthTokenResponse.Success(payload);
        }

        protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
        {
            string stateValue = Options.StateDataFormat.Protect(properties);
            redirectUri = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string?>
            {
                ["appid"] = Options.ClientId,
                ["agentid"] = Options.AgentId,
                ["redirect_uri"] = redirectUri,
                ["state"] = stateValue
            });

            return redirectUri;
        }

        private async Task<(int errCode, string? userId)> GetUserIdentityfierAsync(OAuthTokenResponse tokens)
        {
            var code = Request.Query["code"];
            string address = QueryHelpers.AddQueryString(Options.UserIdentificationEndpoint, new Dictionary<string, string?>
            {
                ["access_token"] = tokens.AccessToken,
                ["code"] = code,
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

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            int errCode = payload.RootElement.TryGetProperty("errcode", out var errCodeElement) && errCodeElement.ValueKind == JsonValueKind.Number ? errCodeElement.GetInt32() : 0;
            return (errCode, userId: payload.RootElement.GetString("UserId"));
        }
    }
}
