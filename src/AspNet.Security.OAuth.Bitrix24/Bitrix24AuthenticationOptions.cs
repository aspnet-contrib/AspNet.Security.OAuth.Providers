/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Bitrix24.Bitrix24AuthenticationConstants;

namespace AspNet.Security.OAuth.Bitrix24
{
    /// <summary>
    /// Defines a set of options used by <see cref="Bitrix24AuthenticationHandler"/>.
    /// </summary>
    public class Bitrix24AuthenticationOptions : OAuthOptions
    {
        public Bitrix24AuthenticationOptions()
        {
            ClaimsIssuer = Bitrix24AuthenticationDefaults.Issuer;
            CallbackPath = Bitrix24AuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = Bitrix24AuthenticationDefaults.AuthorizationEndpointPath;
            TokenEndpoint = Bitrix24AuthenticationDefaults.TokenEndpointPath;
            UserInformationEndpoint = Bitrix24AuthenticationDefaults.UserInformationEndpointPath;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, UserFields.Id);
            ClaimActions.MapJsonKey(ClaimTypes.Email, UserFields.Email);
            ClaimActions.MapCustomJson(ClaimTypes.Name, json => string.Join(' ', json.GetProperty(UserFields.Name).GetString(), json.GetProperty(UserFields.SecondName).GetString(), json.GetProperty(UserFields.LastName).GetString()));
        }

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
