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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Twitter;

/// <summary>
/// Defines a handler for authentication using Twitter.
/// </summary>
public partial class TwitterAuthenticationHandler : OAuthHandler<TwitterAuthenticationOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterAuthenticationHandler"/> class.
    /// </summary>
    /// <param name="options">The authentication options.</param>
    /// <param name="logger">The logger to use.</param>
    /// <param name="encoder">The URL encoder to use.</param>
    public TwitterAuthenticationHandler(
        [NotNull] IOptionsMonitor<TwitterAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    /// <inheritdoc />
    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        [NotNull] ClaimsIdentity identity,
        [NotNull] AuthenticationProperties properties,
        [NotNull] OAuthTokenResponse tokens)
    {
        var endpoint = Options.UserInformationEndpoint;

        endpoint = AddQueryParameterIfValues(endpoint, "expansions", Options.Expansions);
        endpoint = AddQueryParameterIfValues(endpoint, "tweet.fields", Options.TweetFields);
        endpoint = AddQueryParameterIfValues(endpoint, "user.fields", Options.UserFields);

        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

        using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        if (!response.IsSuccessStatusCode)
        {
            await Log.UserProfileErrorAsync(Logger, response, Context.RequestAborted);
            throw new HttpRequestException("An error occurred while retrieving the user profile from Twitter.");
        }

        using var stream = await response.Content.ReadAsStreamAsync(Context.RequestAborted);
        using var payload = await JsonDocument.ParseAsync(stream, cancellationToken: Context.RequestAborted);

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
            ["client_id"] = Options.ClientId,
            ["redirect_uri"] = context.RedirectUri,
            ["client_secret"] = Options.ClientSecret,
            ["code"] = context.Code,
            ["grant_type"] = "authorization_code",
        };

        // PKCE https://tools.ietf.org/html/rfc7636#section-4.5
        if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
        {
            tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
            context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
        }

        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(
            string.Concat(
                Uri.EscapeDataString(Options.ClientId),
                ":",
                Uri.EscapeDataString(Options.ClientSecret))));

        using var requestContent = new FormUrlEncodedContent(tokenRequestParameters!);
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);

        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        requestMessage.Content = requestContent;
        requestMessage.Version = Backchannel.DefaultRequestVersion;

        using var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
        var body = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode switch
        {
            true => OAuthTokenResponse.Success(JsonDocument.Parse(body)),
            false => PrepareFailedOAuthTokenResponse(response, body)
        };

        static OAuthTokenResponse PrepareFailedOAuthTokenResponse(HttpResponseMessage response, string body)
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

    private static string AddQueryParameterIfValues(string endpoint, string name, ISet<string>? values)
    {
        if (values?.Count > 0)
        {
            endpoint = QueryHelpers.AddQueryString(endpoint, name, string.Join(',', values));
        }

        return endpoint;
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
