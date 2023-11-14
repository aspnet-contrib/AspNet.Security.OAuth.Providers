/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AspNet.Security.OAuth.EVEOnline;

public partial class EVEOnlineAuthenticationHandler : OAuthHandler<EVEOnlineAuthenticationOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EVEOnlineAuthenticationHandler"/> class.
    /// </summary>
    /// <param name="options">The authentication options.</param>
    /// <param name="logger">The logger to use.</param>
    /// <param name="encoder">The URL encoder to use.</param>
    public EVEOnlineAuthenticationHandler(
        [NotNull] IOptionsMonitor<EVEOnlineAuthenticationOptions> options,
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
        var accessToken = tokens.AccessToken;

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new AuthenticationFailureException("No access token was returned in the OAuth token.");
        }

        var tokenClaims = ExtractClaimsFromToken(accessToken);

        foreach (var claim in tokenClaims)
        {
            identity.AddClaim(claim);
        }

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, tokens.Response!.RootElement);
        context.RunClaimActions();

        await Events.CreatingTicket(context);
        return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);
    }

    /// <summary>
    /// Extracts the claims from the token received from the token endpoint.
    /// </summary>
    /// <param name="token">The token to extract the claims from.</param>
    /// <returns>
    /// An <see cref="IEnumerable{Claim}"/> containing the claims extracted from the token.
    /// </returns>
    protected virtual IEnumerable<Claim> ExtractClaimsFromToken([NotNull] string token)
    {
        try
        {
            var securityToken = Options.SecurityTokenHandler.ReadJsonWebToken(token);

            var nameClaim = ExtractClaim(securityToken, "name");
            var expClaim = ExtractClaim(securityToken, "exp");

            var claims = new List<Claim>(securityToken.Claims)
            {
                new(ClaimTypes.NameIdentifier, securityToken.Subject.Replace("CHARACTER:EVE:", string.Empty, StringComparison.OrdinalIgnoreCase), ClaimValueTypes.String, ClaimsIssuer),
                new(ClaimTypes.Name, nameClaim.Value, ClaimValueTypes.String, ClaimsIssuer),
                new(ClaimTypes.Expiration, UnixTimeStampToDateTime(expClaim.Value), ClaimValueTypes.DateTime, ClaimsIssuer)
            };

            var scopes = claims.Where(x => string.Equals(x.Type, "scp", StringComparison.OrdinalIgnoreCase)).ToList();

            if (scopes.Count > 0)
            {
                claims.Add(new Claim(EVEOnlineAuthenticationConstants.Claims.Scopes, string.Join(' ', scopes.Select(x => x.Value)), ClaimValueTypes.String, ClaimsIssuer));
            }

            return claims;
        }
        catch (Exception ex)
        {
            throw new AuthenticationFailureException("Failed to parse JWT for claims from EVEOnline token.", ex);
        }
    }

    private static Claim ExtractClaim([NotNull] JsonWebToken token, [NotNull] string claim)
    {
        var extractedClaim = token.Claims.FirstOrDefault(x => string.Equals(x.Type, claim, StringComparison.OrdinalIgnoreCase));
        return extractedClaim ?? throw new AuthenticationFailureException($"The claim '{claim}' is missing from the EVEOnline JWT.");
    }

    private static string UnixTimeStampToDateTime(string unixTimeStamp)
    {
        if (!long.TryParse(unixTimeStamp, NumberStyles.Integer, CultureInfo.InvariantCulture, out var seconds))
        {
            throw new AuthenticationFailureException($"The value {unixTimeStamp} of the 'exp' claim is not a valid 64-bit integer.");
        }

        DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds(seconds);
        return offset.ToString("o", CultureInfo.InvariantCulture);
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
