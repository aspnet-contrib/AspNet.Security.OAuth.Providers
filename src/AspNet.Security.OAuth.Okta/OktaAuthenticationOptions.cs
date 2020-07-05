﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Okta
{
    /// <summary>
    /// Defines a set of options used by <see cref="OktaAuthenticationHandler"/>.
    /// </summary>
    public class OktaAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OktaAuthenticationOptions"/> class.
        /// </summary>
        public OktaAuthenticationOptions()
        {
            ClaimsIssuer = OktaAuthenticationDefaults.Issuer;
            CallbackPath = OktaAuthenticationDefaults.CallbackPath;

            Scope.Add("openid");
            Scope.Add("profile");
            Scope.Add("email");

            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
        }

        /// <summary>
        /// Gets or sets the Okta domain (Org URL) to use for authentication.
        /// </summary>
        public string? Domain { get; set; }

        /// <inheritdoc/>
        public override void Validate()
        {
            base.Validate();

            if (!Uri.TryCreate(AuthorizationEndpoint, UriKind.Absolute, out var _))
            {
                throw new ArgumentException(
                    $"The '{nameof(AuthorizationEndpoint)}' option must be set to a valid URI.",
                    nameof(AuthorizationEndpoint));
            }

            if (!Uri.TryCreate(TokenEndpoint, UriKind.Absolute, out var _))
            {
                throw new ArgumentException(
                    $"The '{nameof(TokenEndpoint)}' option must be set to a valid URI.",
                    nameof(TokenEndpoint));
            }

            if (!Uri.TryCreate(UserInformationEndpoint, UriKind.Absolute, out var _))
            {
                throw new ArgumentException(
                    $"The '{nameof(UserInformationEndpoint)}' option must be set to a valid URI.",
                    nameof(UserInformationEndpoint));
            }
        }
    }
}
