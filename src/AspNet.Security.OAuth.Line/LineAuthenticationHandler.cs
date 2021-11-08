﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Line;

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

    protected override string BuildChallengeUrl([NotNull] AuthenticationProperties properties, [NotNull] string redirectUri)
        => QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, new Dictionary<string, string?>
        {
            ["response_type"] = "code",
            ["client_id"] = Options.ClientId,
            ["redirect_uri"] = redirectUri,
            ["state"] = Options.StateDataFormat.Protect(properties),
            ["scope"] = FormatScope(),
            ["prompt"] = Options.Prompt ? "consent" : string.Empty
        });

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var parameters = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = context.Code,
            ["redirect_uri"] = context.RedirectUri,
            ["client_id"] = Options.ClientId,
            ["client_secret"] = Options.ClientSecret
        };
        request.Content = new FormUrlEncodedContent(parameters!);

        using var response = await Backchannel.SendAsync(request, Context.RequestAborted);
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
                            /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

            throw new HttpRequestException("An error occurred while retrieving user information.");
        }

        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
        context.RunClaimActions();

        // When the email address is not public, retrieve it from the emails endpoint if the user:email scope is specified.
        if (!string.IsNullOrEmpty(Options.UserEmailsEndpoint) && Options.Scope.Contains("email"))
        {
            string? email = await GetEmailAsync(tokens);

            if (!string.IsNullOrEmpty(email))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, Options.ClaimsIssuer));
            }
        }

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    protected virtual async Task<string?> GetEmailAsync([NotNull] OAuthTokenResponse tokens)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, Options.UserEmailsEndpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var parameters = new Dictionary<string, string?>
        {
            ["id_token"] = tokens.Response?.RootElement.GetString("id_token") ?? string.Empty,
            ["client_id"] = Options.ClientId
        };
        request.Content = new FormUrlEncodedContent(parameters!);

        using var response = await Backchannel.SendAsync(request, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            Logger.LogWarning("An error occurred while retrieving the email address associated with the logged in user: " +
                              "the remote server returned a {Status} response with the following payload: {Headers} {Body}.",
                              /* Status: */ response.StatusCode,
                              /* Headers: */ response.Headers.ToString(),
                              /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

            throw new HttpRequestException("An error occurred while retrieving the email address associated to the user profile.");
        }

        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
        return payload.RootElement.GetString("email");
    }
}
