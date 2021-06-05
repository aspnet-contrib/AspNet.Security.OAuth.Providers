/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Moodle.MoodleAuthenticationConstants;

namespace AspNet.Security.OAuth.Moodle
{
    /// <summary>
    /// Defines a set of options used by <see cref="MoodleAuthenticationHandler"/>.
    /// </summary>
    public class MoodleAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoodleAuthenticationOptions"/> class.
        /// </summary>
        public MoodleAuthenticationOptions()
        {
            ClaimsIssuer = MoodleAuthenticationDefaults.Issuer;
            CallbackPath = MoodleAuthenticationDefaults.CallbackPath;

            Scope.Add("user_info");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "username");

            ClaimActions.MapCustomJson(ClaimTypes.Name,
                e => e.GetString("lang")?.StartsWith("zh", StringComparison.OrdinalIgnoreCase) ?? false
                      ? e.GetString("lastname") + e.GetString("firstname")
                      : e.GetString("firstname") + " " + e.GetString("lastname"));
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.MobilePhone, "phone1");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstname");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastname");
            ClaimActions.MapJsonKey(ClaimTypes.AuthenticationMethod, "auth");
            ClaimActions.MapJsonKey(ClaimTypes.Country, "country");
            ClaimActions.MapJsonKey(Claims.IdNumber, "idnumber");
            ClaimActions.MapJsonKey(Claims.MoodleId, "id");
            ClaimActions.MapJsonKey(Claims.Language, "lang");
            ClaimActions.MapJsonKey(Claims.Description, "description");
        }

        /// <summary>
        /// Gets or sets the Moodle domain (Org URL) to use for authentication.
        /// For example: 'moodledomain.com'.
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
