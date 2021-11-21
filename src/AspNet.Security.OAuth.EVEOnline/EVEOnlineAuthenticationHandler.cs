﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.EVEOnline;

public partial class EVEOnlineAuthenticationHandler : OAuthHandler<EVEOnlineAuthenticationOptions>
{
    public EVEOnlineAuthenticationHandler(
        [NotNull] IOptionsMonitor<EVEOnlineAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder,
        [NotNull] ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }
    
    private readonly JwtSecurityTokenHandler _tokenHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="EVEOnlineAuthenticationHandler"/> class.
    /// </summary>
    /// <param name="options">The authentication options.</param>
    /// <param name="logger">The logger to use.</param>
    /// <param name="encoder">The URL encoder to use.</param>
    /// <param name="clock">The system clock to use.</param>
    /// <param name="tokenHandler">The JWT security token handler to use.</param>
    public EVEOnlineAuthenticationHandler(
        [NotNull] IOptionsMonitor<EVEOnlineAuthenticationOptions> options,
        [NotNull] ILoggerFactory logger,
        [NotNull] UrlEncoder encoder,
        [NotNull] ISystemClock clock,
        [NotNull] JwtSecurityTokenHandler tokenHandler)
        : base(options, logger, encoder, clock)
    {
        _tokenHandler = tokenHandler;
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(
          [NotNull] ClaimsIdentity identity,
          [NotNull] AuthenticationProperties properties,
          [NotNull] OAuthTokenResponse tokens)
    {
        var tokenClaims = ExtractClaimsFromToken(tokens.AccessToken);

        foreach (var claim in tokenClaims)
        {
            identity.AddClaim(claim);
        }

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, tokens.Response.RootElement);
        context.RunClaimActions();

        await Options.Events.CreatingTicket(context);
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
            var securityToken = _tokenHandler.ReadJwtToken(token);

            var nameClaim = ExtractClaim(securityToken, "name");
            var expClaim = ExtractClaim(securityToken, "exp");

            var claims = new List<Claim>(securityToken.Claims);

            claims.Add(new Claim(ClaimTypes.NameIdentifier, securityToken.Subject.Replace("CHARACTER:EVE:", string.Empty, StringComparison.OrdinalIgnoreCase), ClaimValueTypes.String, ClaimsIssuer));
            claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value, ClaimValueTypes.String, ClaimsIssuer));
            claims.Add(new Claim(ClaimTypes.Expiration, UnixTimeStampToDateTime(expClaim.Value), ClaimValueTypes.DateTime, ClaimsIssuer));

            var scopes = claims.Where(x => string.Equals(x.Type, "scp", StringComparison.OrdinalIgnoreCase)).ToList();

            if (scopes.Any())
            {
                claims.RemoveAll(x => scopes.Contains(x));

                claims.Add(new Claim(EVEOnlineAuthenticationConstants.Claims.Scopes, string.Join(" ", scopes.Select(x => x.Value)), ClaimValueTypes.String, ClaimsIssuer));
            }

            return claims;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to parse JWT for claims from EVEOnline token.", ex);
        }
    }

    private static Claim ExtractClaim(JwtSecurityToken securityToken, string claim)
    {
        var extractedClaim = securityToken.Claims.FirstOrDefault(x => string.Equals(x.Type, claim, StringComparison.OrdinalIgnoreCase));

        if (extractedClaim == null)
        {
            throw new InvalidOperationException($"The claim '{claim}' is missing from the EVEOnline JWT.");
        }

        return extractedClaim;
    }

    private static string UnixTimeStampToDateTime(string unixTimeStamp)
    {
        if (!long.TryParse(unixTimeStamp, NumberStyles.Any, CultureInfo.InvariantCulture, out long unixTime))
        {
            throw new InvalidOperationException($"The value {unixTimeStamp} of the 'exp' claim is not a valid 64-bit integer.");
        }

        DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
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
