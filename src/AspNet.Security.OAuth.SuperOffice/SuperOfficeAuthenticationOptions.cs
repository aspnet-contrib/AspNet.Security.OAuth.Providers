/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Defines a set of options used by <see cref="SuperOfficeAuthenticationHandler"/>.
    /// </summary>
    public class SuperOfficeAuthenticationOptions : OAuthOptions
    {
        private SuperOfficeAuthenticationEnvironment _environment;

        /// <summary>
        /// Initializes a new <see cref="SuperOfficeAuthenticationOptions"/>. Default environment is Development (SOD).
        /// </summary>
        public SuperOfficeAuthenticationOptions()
        {
            Environment = SuperOfficeAuthenticationEnvironment.Development;

            CallbackPath = new PathString(SuperOfficeAuthenticationDefaults.CallbackPath);

            Events = new SuperOfficeAuthenticationEvents();

            Scope.Add("openid");

            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.ClaimNames.AssociateId, SuperOfficeAuthenticationConstants.PrincipalNames.AssociateId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.ClaimNames.UserPrincipalName, SuperOfficeAuthenticationConstants.PrincipalNames.Associate);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.ClaimNames.Email, SuperOfficeAuthenticationConstants.PrincipalNames.EMailAddress);

            ClaimActions.MapJsonKey(ClaimTypes.Name, SuperOfficeAuthenticationConstants.PrincipalNames.FullName);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.BusinessId, SuperOfficeAuthenticationConstants.PrincipalNames.BusinessId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.CategoryId, SuperOfficeAuthenticationConstants.PrincipalNames.CategoryId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.ContactId, SuperOfficeAuthenticationConstants.PrincipalNames.ContactId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.ContextIdentifier, SuperOfficeAuthenticationConstants.PrincipalNames.DatabaseContextIdentifier);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.HomeCountryId, SuperOfficeAuthenticationConstants.PrincipalNames.HomeCountryId);

            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.RoleName, SuperOfficeAuthenticationConstants.PrincipalNames.RoleName);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.RoleId, SuperOfficeAuthenticationConstants.PrincipalNames.RoleId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.CountryId, SuperOfficeAuthenticationConstants.PrincipalNames.CountryId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.GroupId, SuperOfficeAuthenticationConstants.PrincipalNames.GroupId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.SecondaryGroups, SuperOfficeAuthenticationConstants.PrincipalNames.SecondaryGroups);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.PersonId, SuperOfficeAuthenticationConstants.PrincipalNames.PersonId);
        }

        /// <summary>
        /// Gets or sets the <see cref="SuperOfficeAuthenticationEvents"/> used to handle authentication events.
        /// </summary>
        public new SuperOfficeAuthenticationEvents Events
        {
            get => (SuperOfficeAuthenticationEvents)base.Events;
            set => base.Events = value;
        }

        /// <summary>
        /// Gets or sets the URI the middleware will access to obtain the public key for
        /// validating tokens if <see cref="ValidateTokens"/> is <see langword="true"/>.
        /// </summary>
        public string ConfigurationEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Sets the target online environment to either sod, stage or online
        /// </summary>
        public SuperOfficeAuthenticationEnvironment Environment
        {
            get
            {
                return this._environment;
            }

            set
            {
                _environment = value;
                UpdateEndpoints();
            }
        }

        /// <summary>
        /// When true user claims will include SuperOffice function rights.
        /// </summary>
        public bool IncludeFunctionalRightsAsClaims { get; set; }

        /// <summary>
        /// When true user claims will include SuperOffice id_token claims.
        /// </summary>
        public bool IncludeIdTokenAsClaims { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to validate tokens using SuperOffice's public key.
        /// </summary>
        public bool ValidateTokens { get; set; } = true;

        /// <summary>
        /// Gets or sets the default period of time to cache the SuperOffice public key(s)
        /// retrieved from the endpoint specified by <see cref="ConfigurationEndpoint"/>.
        /// </summary>
        /// <remarks>
        /// The default public key cache lifetime is 15 minutes.
        /// </remarks>
        public TimeSpan PublicKeyCacheLifetime { get; set; } = TimeSpan.FromMinutes(15);

        /// <inheritdoc />
        public override void Validate()
        {
            base.Validate();

            if (_environment == SuperOfficeAuthenticationEnvironment.Production
                && !AuthorizationEndpoint.StartsWith("https://", System.StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Production environment requires secure endpoints, i.e. begins with 'https://'.");
            }
        }

        /// <summary>
        /// Updates configuration settings.
        /// </summary>
        private void UpdateEndpoints()
        {
            var env = GetEnvironment();

            AuthorizationEndpoint = string.Format(CultureInfo.InvariantCulture,
                SuperOfficeAuthenticationConstants.FormatStrings.AuthorizeEndpoint,
                env);

            TokenEndpoint = string.Format(CultureInfo.InvariantCulture,
                SuperOfficeAuthenticationConstants.FormatStrings.TokenEndpoint,
                env);

            ClaimsIssuer = string.Format(CultureInfo.InvariantCulture,
                SuperOfficeAuthenticationConstants.FormatStrings.ClaimsIssuer,
                env);

            /* UserInformationEndpoint will include context identifier after authentication
               in SuperOfficeAuthenticationHandler.CreateTicketAsync */

            UserInformationEndpoint = string.Concat(
                string.Format(CultureInfo.InvariantCulture, SuperOfficeAuthenticationConstants.FormatStrings.ClaimsIssuer, env),
                SuperOfficeAuthenticationConstants.FormatStrings.UserInfoEndpoint);

            ConfigurationEndpoint = string.Format(CultureInfo.InvariantCulture,
                SuperOfficeAuthenticationConstants.FormatStrings.ConfigurationEndpoint,
                env);
        }

        /// <summary>
        /// Used to determine current target environment.
        /// </summary>
        /// <returns>Returns the online environment, i.e. development, stage, production.</returns>
        private string GetEnvironment()
        {
            return _environment switch
            {
                SuperOfficeAuthenticationEnvironment.Development => "sod",
                SuperOfficeAuthenticationEnvironment.Stage => "stage",
                SuperOfficeAuthenticationEnvironment.Production => "online",
                _ => throw new NotSupportedException("Environment property must be set to either Development, Stage or Production.")
            };
        }
    }
}
