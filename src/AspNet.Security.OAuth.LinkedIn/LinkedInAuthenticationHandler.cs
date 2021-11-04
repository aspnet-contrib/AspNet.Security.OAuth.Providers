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
using Microsoft.Extensions.Primitives;

namespace AspNet.Security.OAuth.LinkedIn
{
    public class LinkedInAuthenticationHandler : OAuthHandler<LinkedInAuthenticationOptions>
    {
        public LinkedInAuthenticationHandler(
            [NotNull] IOptionsMonitor<LinkedInAuthenticationOptions> options,
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
            string requestUri = Options.UserInformationEndpoint;
            var fields = Options.Fields
                .Where(f => !string.Equals(f, LinkedInAuthenticationConstants.EmailAddressField, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // If at least one field is specified, append the fields to the endpoint URL.
            if (fields.Count > 0)
            {
                requestUri = QueryHelpers.AddQueryString(requestUri, "projection", $"({string.Join(',', fields)})");
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("x-li-format", "json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            using var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            if (!string.IsNullOrEmpty(Options.EmailAddressEndpoint) &&
                Options.Fields.Contains(LinkedInAuthenticationConstants.EmailAddressField))
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
            using var request = new HttpRequestMessage(HttpMethod.Get, Options.EmailAddressEndpoint);
            request.Headers.Add("x-li-format", "json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            using var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the email address: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));

                throw new HttpRequestException("An error occurred while retrieving the email address.");
            }

            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));

            if (!payload.RootElement.TryGetProperty("elements", out var emails))
            {
                return null;
            }

            return emails
                .EnumerateArray()
                .Select((p) => p.GetProperty("handle~"))
                .Select((p) => p.GetString("emailAddress"))
                .FirstOrDefault();
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            // Taken from the base class' implementation so we can modify the status code check
            // for "access denied" as LinkedIn uses non-standard error codes (see https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/480).
            // https://github.com/dotnet/aspnetcore/blob/bbf7c8780c42e3d32aeec8018367037784eb9181/src/Security/Authentication/OAuth/src/OAuthHandler.cs#L49-L87
            var query = Request.Query;

            var state = query["state"];
            var properties = Options.StateDataFormat.Unprotect(state);

            if (properties == null)
            {
                return HandleRequestResult.Fail("The oauth state was missing or invalid.");
            }

            // OAuth2 10.12 CSRF
            if (!ValidateCorrelationId(properties))
            {
                return HandleRequestResult.Fail("Correlation failed.", properties);
            }

            var error = query["error"];
            if (!StringValues.IsNullOrEmpty(error))
            {
                // Note: access_denied errors are special protocol errors indicating the user didn't
                // approve the authorization demand requested by the remote authorization server.
                // Since it's a frequent scenario (that is not caused by incorrect configuration),
                // denied errors are handled differently using HandleAccessDeniedErrorAsync().
                // Visit https://tools.ietf.org/html/rfc6749#section-4.1.2.1 for more information.
                var errorDescription = query["error_description"];
                var errorUri = query["error_uri"];

                // See https://docs.microsoft.com/en-us/linkedin/shared/authentication/authorization-code-flow#application-is-rejected
                if (StringValues.Equals(error, "access_denied") ||
                    StringValues.Equals(error, "user_cancelled_login") ||
                    StringValues.Equals(error, "user_cancelled_authorize"))
                {
                    var result = await HandleAccessDeniedErrorAsync(properties);
                    if (!result.None)
                    {
                        return result;
                    }

                    var deniedEx = new Exception("Access was denied by the resource owner or by the remote server.");
                    deniedEx.Data["error"] = error.ToString();
                    deniedEx.Data["error_description"] = errorDescription.ToString();
                    deniedEx.Data["error_uri"] = errorUri.ToString();

                    return HandleRequestResult.Fail(deniedEx, properties);
                }
            }

            return await base.HandleRemoteAuthenticateAsync();
        }
    }
}
