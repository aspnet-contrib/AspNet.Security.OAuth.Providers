/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Gitee;

public partial class GiteeAuthenticationHandler : OAuthHandler<GiteeAuthenticationOptions>
{
    public GiteeAuthenticationHandler(
        [NotNull] IOptionsMonitor<GiteeAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder)
        : base(options, logger, encoder)
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
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using var payload = await JsonDocument.ParseAsync(stream, cancellationToken: Context.RequestAborted);

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
        context.RunClaimActions();

        // When the email address is not public, retrieve it from
        // the emails endpoint if the user:email scope is specified.
        if (!string.IsNullOrEmpty(Options.UserEmailsEndpoint) &&
            !identity.HasClaim(claim => claim.Type == ClaimTypes.Email) &&
            Options.Scope.Contains("emails"))
        {
            var address = await GetEmailAsync(tokens);

            if (!string.IsNullOrEmpty(address))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, address, ClaimValueTypes.String, Options.ClaimsIssuer));
            }
        }

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    protected async Task<string?> GetEmailAsync([NotNull] OAuthTokenResponse tokens)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, Options.UserEmailsEndpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.EmailAddressErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the email address associated to the user profile.");
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using var payload = await JsonDocument.ParseAsync(stream, cancellationToken: Context.RequestAborted);

        return (from address in payload.RootElement.EnumerateArray()
                select address.GetString("email")).FirstOrDefault();
    }

    private static partial class Log
    {
        internal static async Task UserProfileErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            UserProfileError(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        internal static async Task EmailAddressErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            EmailAddressError(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        [LoggerMessage(1, LogLevel.Error, "An error occurred while retrieving the user profile: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void UserProfileError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);

        [LoggerMessage(2, LogLevel.Warning, "An error occurred while retrieving the email address associated with the logged in user: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void EmailAddressError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);
    }
}
