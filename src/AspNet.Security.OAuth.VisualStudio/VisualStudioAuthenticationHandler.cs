﻿/*
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
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.VisualStudio
{
    public class VisualStudioAuthenticationHandler : OAuthHandler<VisualStudioAuthenticationOptions>
    {
        public VisualStudioAuthenticationHandler(
            [NotNull] IOptionsMonitor<VisualStudioAuthenticationOptions> options,
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
            using var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            var parameters = new Dictionary<string, string>
            {
                ["redirect_uri"] = context.RedirectUri,
                ["client_assertion"] = Options.ClientSecret,
                ["assertion"] = context.Code,
                ["grant_type"] = "urn:ietf:params:oauth:grant-type:jwt-bearer",
                ["client_assertion_type"] = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer"
            };

            request.Content = new FormUrlEncodedContent(parameters);

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

        protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
        {
            return QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string>
            {
                ["client_id"] = Options.ClientId,
                ["response_type"] = "Assertion",
                ["scope"] = FormatScope(),
                ["redirect_uri"] = redirectUri,
                ["state"] = Options.StateDataFormat.Protect(properties),
            });
        }
    }
}
