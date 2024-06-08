/*
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

namespace AspNet.Security.OAuth.DigitalOcean;

public partial class DigitalOceanAuthenticationHandler : OAuthHandler<DigitalOceanAuthenticationOptions>
{
    public DigitalOceanAuthenticationHandler(
        [NotNull] IOptionsMonitor<DigitalOceanAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        // See https://www.digitalocean.com/community/tutorials/how-to-use-oauth-authentication-with-digitalocean-as-a-user-or-developer for details
        var tokenRequestParameters = new Dictionary<string, string?>()
        {
            ["client_id"] = Options.ClientId,
            ["client_secret"] = Options.ClientSecret,
            ["redirect_uri"] = context.RedirectUri,
            ["code"] = context.Code,
            ["grant_type"] = "authorization_code",
        };

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, tokenRequestParameters);

        using var request = new HttpRequestMessage(HttpMethod.Post, address);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.ExchangeCodeAsync(Logger, response, Context.RequestAborted);
            return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        var payload = await JsonDocument.ParseAsync(stream);
        return OAuthTokenResponse.Success(payload);
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
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using var payload = await JsonDocument.ParseAsync(stream, cancellationToken: Context.RequestAborted);

        var account = payload.RootElement.GetProperty("account");

        identity.AddClaim(new Claim(DigitalOceanAuthenticationConstants.Claims.EmailVerified, account.GetProperty("email_verified").GetBoolean().ToString().ToLowerInvariant()));

        var principal = new ClaimsPrincipal(identity);
        var user = tokens.Response!.RootElement.GetProperty("info");

        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, user);

        context.RunClaimActions();
        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    private static partial class Log
    {
        internal static async Task UserProfileErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            UserProfileErrorAsync(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        [LoggerMessage(2, LogLevel.Error, "An error occurred while retrieving profile data: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        static partial void UserProfileErrorAsync(
            ILogger logger,
            System.Net.HttpStatusCode status,
            string headers,
            string body);

        internal static async Task ExchangeCodeAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            ExchangeCodeAsync(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        [LoggerMessage(1, LogLevel.Error, "An error occurred while retrieving an access token: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        static partial void ExchangeCodeAsync(
            ILogger logger,
            System.Net.HttpStatusCode status,
            string headers,
            string body);
    }
}
