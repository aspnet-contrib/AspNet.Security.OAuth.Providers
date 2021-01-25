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
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Line
{
    public class LineAuthenticationHandler : OAuthHandler<LineAuthenticationOptions>
    {
        public LineAuthenticationHandler(
            [NotNull] IOptionsMonitor<LineAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        // private const string OauthState = "_oauthstate";
        // private const string State = "state";
        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            return await base.HandleRemoteAuthenticateAsync();
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await base.HandleChallengeAsync(properties);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            return base.BuildChallengeUrl(properties, redirectUri);
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await Backchannel.SendAsync(request, Context.RequestAborted);
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
            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = context.Code,
                ["redirect_uri"] = context.RedirectUri,
                ["client_id"] = Options.ClientId,
                ["client_secret"] = Options.ClientSecret
            });

            using var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return OAuthTokenResponse.Success(payload);
        }

        // protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
        // {
        //     string stateValue = Options.StateDataFormat.Protect(properties);
        //     bool addRedirectHash = false;
        //
        //     if (!IsWeixinAuthorizationEndpointInUse())
        //     {
        //         // Store state in redirectUri when authorizing Wechat Web pages to prevent "too long state parameters" error
        //         redirectUri = QueryHelpers.AddQueryString(redirectUri, OauthState, stateValue);
        //         addRedirectHash = true;
        //     }
        //
        //     redirectUri = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
        //     {
        //         ["appid"] = Options.ClientId,
        //         ["scope"] = FormatScope(),
        //         ["response_type"] = "code",
        //         ["redirect_uri"] = redirectUri,
        //         [State] = addRedirectHash ? OauthState : stateValue
        //     });
        //
        //     if (addRedirectHash)
        //     {
        //         // The parameters necessary for Web Authorization of Wechat
        //         redirectUri += "#wechat_redirect";
        //     }
        //
        //     return redirectUri;
        // }

        // protected override string FormatScope() => string.Join(",", Options.Scope);

        // private bool IsWeixinAuthorizationEndpointInUse()
        // {
        //     return string.Equals(Options.AuthorizationEndpoint, LineAuthenticationDefaults.AuthorizationEndpoint, StringComparison.OrdinalIgnoreCase);
        // }
    }
}
