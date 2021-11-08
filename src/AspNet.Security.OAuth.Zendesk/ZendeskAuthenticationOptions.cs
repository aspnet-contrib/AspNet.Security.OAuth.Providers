/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Zendesk
{
    public class ZendeskAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Defines a set of options used by <see cref="ZendeskAuthenticationHandler"/>.
        /// </summary>
        public ZendeskAuthenticationOptions()
        {
            ClaimsIssuer = ZendeskAuthenticationDefaults.Issuer;
            CallbackPath = ZendeskAuthenticationDefaults.CallbackPath;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

            Scope.Add("read");
        }

        /// <summary>
        /// Gets or sets the Zendesk domain name.
        /// For example: <c>glowingwaffle.zendesk.com</c>.
        /// </summary>
        public string? Domain { get; set; }

        /// <inheritdoc/>
        public override void Validate()
        {
            base.Validate();

            if (!Uri.TryCreate(AuthorizationEndpoint, UriKind.Absolute, out _))
            {
                throw new ArgumentException(
                    $"The '{nameof(AuthorizationEndpoint)}' option must be set to a valid URI.",
                    nameof(AuthorizationEndpoint));
            }

            if (!Uri.TryCreate(TokenEndpoint, UriKind.Absolute, out _))
            {
                throw new ArgumentException(
                    $"The '{nameof(TokenEndpoint)}' option must be set to a valid URI.",
                    nameof(TokenEndpoint));
            }

            if (!Uri.TryCreate(UserInformationEndpoint, UriKind.Absolute, out _))
            {
                throw new ArgumentException(
                    $"The '{nameof(UserInformationEndpoint)}' option must be set to a valid URI.",
                    nameof(UserInformationEndpoint));
            }
        }
    }
}
