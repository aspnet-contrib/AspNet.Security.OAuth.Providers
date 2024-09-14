/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Zoho;

public partial class ZohoAuthenticationHandler : OAuthHandler<ZohoAuthenticationOptions>
{
    public ZohoAuthenticationHandler(
        [NotNull] IOptionsMonitor<ZohoAuthenticationOptions> options,
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
        var userInformationEndpoint = CreateEndpoint(ZohoAuthenticationDefaults.UserInformationPath);
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, userInformationEndpoint);
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
        requestMessage.Version = Backchannel.DefaultRequestVersion;

        using var response = await Backchannel.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile.");
        }

        using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
        context.RunClaimActions();

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] OAuthCodeExchangeContext context)
    {
        var nameValueCollection = new Dictionary<string, string?>
        {
            ["client_id"] = Options.ClientId,
            ["client_secret"] = Options.ClientSecret,
            ["code"] = context.Code,
            ["redirect_uri"] = context.RedirectUri,
            ["grant_type"] = "authorization_code"
        };

        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            nameValueCollection.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        var tokenEndpoint = CreateEndpoint(ZohoAuthenticationDefaults.TokenPath);
        using var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        request.Content = new FormUrlEncodedContent(nameValueCollection);
        request.Version = Backchannel.DefaultRequestVersion;

        using var response = await Backchannel.SendAsync(request, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.ExchangeCodeErrorAsync(Logger, response, Context.RequestAborted);
            return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
        }

        var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

        return OAuthTokenResponse.Success(payload);
    }

    /// <summary>
    /// Creates the endpoint for the Zoho API using the location parameter.
    /// If the location parameter doesn't match any of the supported locations, the default location (US) is used.
    /// We don't use the <c>accounts-server</c> parameter for security reasons.
    /// </summary>
    /// <param name="path">The request path.</param>
    /// <returns>The API endpoint for the Zoho API.</returns>
    private string CreateEndpoint(string path)
    {
        var location = Context.Request.Query["location"];

        var domain = location.ToString().ToLowerInvariant() switch
        {
            "au" => "https://accounts.zoho.com.au",
            "ca" => "https://accounts.zohocloud.ca",
            "eu" => "https://accounts.zoho.eu",
            "us" => "https://accounts.zoho.com",
            "in" => "https://accounts.zoho.in",
            "jp" => "https://accounts.zoho.jp",
            "sa" => "https://accounts.zoho.sa",
            "uk" => "https://accounts.zoho.uk",
            _ => "https://accounts.zoho.com"
        };

        var builder = new UriBuilder(domain)
        {
            Path = path,
            Port = -1,
            Scheme = Uri.UriSchemeHttps,
        };

        return builder.Uri.ToString();
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

        internal static async Task ServerInfoErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            ServerInfoErrorAsync(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        internal static async Task ExchangeCodeErrorAsync(ILogger logger, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            ExchangeCodeError(
                logger,
                response.StatusCode,
                response.Headers.ToString(),
                await response.Content.ReadAsStringAsync(cancellationToken));
        }

        [LoggerMessage(1, LogLevel.Error, "An error occurred while retrieving the user profile: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void UserProfileError(
            ILogger logger,
            System.Net.HttpStatusCode status,
            string headers,
            string body);

        [LoggerMessage(2, LogLevel.Error, "An error occurred while retrieving the server info: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void ServerInfoErrorAsync(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);

        [LoggerMessage(3, LogLevel.Error, "An error occurred while retrieving an access token: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void ExchangeCodeError(
            ILogger logger,
            HttpStatusCode status,
            string headers,
            string body);
    }
}
