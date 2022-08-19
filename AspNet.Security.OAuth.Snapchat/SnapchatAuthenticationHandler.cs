/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Snapchat;

public partial class SnapchatAuthenticationHandler : OAuthHandler<SnapchatAuthenticationOptions>
{
    public SnapchatAuthenticationHandler(
        [NotNull] IOptionsMonitor<SnapchatAuthenticationOptions> options,
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
        string endpoint = Options.UserInformationEndpoint;

        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile from Snapchat.");
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
        var tokenRequestParameters = new Dictionary<string, string>()
        {
            ["code"] = context.Code,
            ["client_id"] = Options.ClientId,
            ["client_secret"] = Options.ClientSecret,
            ["grant_type"] = "authorization_code",
            ["redirect_uri"] = context.RedirectUri,
        };

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        using var requestContent = new FormUrlEncodedContent(tokenRequestParameters!);
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);

        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        requestMessage.Content = requestContent;
        requestMessage.Version = Backchannel.DefaultRequestVersion;

        using var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
        var body = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode switch
        {
            true => OAuthTokenResponse.Success(JsonDocument.Parse(body)),
            false => PrepareFailedOAuthTokenReponse(response, body)
        };

        static OAuthTokenResponse PrepareFailedOAuthTokenReponse(HttpResponseMessage response, string body)
        {
            var exception = GetStandardErrorException(JsonDocument.Parse(body));

            if (exception is null)
            {
                var errorMessage = $"OAuth token endpoint failure: Status: {response.StatusCode};Headers: {response.Headers};Body: {body};";
                return OAuthTokenResponse.Failed(new Exception(errorMessage));
            }

            return OAuthTokenResponse.Failed(exception);

            static Exception? GetStandardErrorException(JsonDocument response)
            {
                var root = response.RootElement;
                var error = root.GetString("error");

                if (error is null)
                {
                    return null;
                }

                var result = new StringBuilder("OAuth token endpoint failure: ");
                result.Append(error);

                if (root.TryGetProperty("error_description", out var errorDescription))
                {
                    result.Append(";Description=");
                    result.Append(errorDescription);
                }

                if (root.TryGetProperty("error_uri", out var errorUri))
                {
                    result.Append(";Uri=");
                    result.Append(errorUri);
                }

                var exception = new Exception(result.ToString());
                exception.Data["error"] = error.ToString();
                exception.Data["error_description"] = errorDescription.ToString();
                exception.Data["error_uri"] = errorUri.ToString();

                return exception;
            }
        }
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

        [LoggerMessage(1, LogLevel.Error, "An error occurred while retrieving the user profile: the remote server returned a {Status} response with the following payload: {Headers} {Body}.")]
        private static partial void UserProfileError(
            ILogger logger,
            System.Net.HttpStatusCode status,
            string headers,
            string body);
    }
}
