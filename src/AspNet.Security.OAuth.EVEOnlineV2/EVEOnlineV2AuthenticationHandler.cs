/*
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
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.EVEOnlineV2
{
    public class EVEOnlineV2AuthenticationHandler : OAuthHandler<EVEOnlineV2AuthenticationOptions>
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="EVEOnlineV2AuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The authentication options.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="encoder">The URL encoder to use.</param>
        /// <param name="clock">The system clock to use.</param>
        /// <param name="tokenHandler">The JWT security token handler to use.</param>
        public EVEOnlineV2AuthenticationHandler(
            [NotNull] IOptionsMonitor<EVEOnlineV2AuthenticationOptions> options,
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
            // JwtSecurityToken decodedToken = new JwtSecurityToken(tokens.AccessToken);
            var tokenClaims = ExtractClaimsFromToken(tokens.AccessToken);

            foreach (var claim in tokenClaims)
            {
                identity.AddClaim(claim);
            }

            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, tokens.Response.RootElement);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
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

                return new List<Claim>(securityToken.Claims)
                {
                    new Claim(ClaimTypes.NameIdentifier, securityToken.Subject.Replace("CHARACTER:EVE:", string.Empty, StringComparison.OrdinalIgnoreCase), ClaimValueTypes.String, ClaimsIssuer),
                    new Claim(ClaimTypes.GivenName, securityToken.Claims.First(x => x.Type.Equals("name", StringComparison.OrdinalIgnoreCase)).Value, ClaimValueTypes.String, ClaimsIssuer),
                    new Claim(ClaimTypes.Name, securityToken.Claims.First(x => x.Type.Equals("name", StringComparison.OrdinalIgnoreCase)).Value, ClaimValueTypes.String, ClaimsIssuer),
                    new Claim(ClaimTypes.Expiration, UnixTimeStampToDateTime(securityToken.Claims.First(x => x.Type.Equals("exp", StringComparison.OrdinalIgnoreCase)).Value), ClaimValueTypes.DateTime, ClaimsIssuer),
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to parse JWT for claims from EVEOnlineV2 token.", ex);
            }
        }

        public static string UnixTimeStampToDateTime(string unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(double.Parse(unixTimeStamp, CultureInfo.InvariantCulture));
            return dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'", CultureInfo.InvariantCulture);
        }
    }
}
