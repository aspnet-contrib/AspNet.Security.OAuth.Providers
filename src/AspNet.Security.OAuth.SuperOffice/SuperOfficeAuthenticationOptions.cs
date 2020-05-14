/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

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
            CallbackPath = SuperOfficeAuthenticationDefaults.CallbackPath;
            Scope.Add("openid");

            ClaimActions.MapJsonKey(ClaimTypes.Name, SuperOfficeAuthenticationConstants.PrincipalNames.FullName);

            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.ClaimNames.AssociateId, SuperOfficeAuthenticationConstants.PrincipalNames.AssociateId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.ClaimNames.Email, SuperOfficeAuthenticationConstants.PrincipalNames.EMailAddress);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.ClaimNames.UserPrincipalName, SuperOfficeAuthenticationConstants.PrincipalNames.Associate);

            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.BusinessId, SuperOfficeAuthenticationConstants.PrincipalNames.BusinessId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.CategoryId, SuperOfficeAuthenticationConstants.PrincipalNames.CategoryId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.ContactId, SuperOfficeAuthenticationConstants.PrincipalNames.ContactId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.ContextIdentifier, SuperOfficeAuthenticationConstants.PrincipalNames.DatabaseContextIdentifier);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.CountryId, SuperOfficeAuthenticationConstants.PrincipalNames.CountryId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.GroupId, SuperOfficeAuthenticationConstants.PrincipalNames.GroupId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.HomeCountryId, SuperOfficeAuthenticationConstants.PrincipalNames.HomeCountryId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.PersonId, SuperOfficeAuthenticationConstants.PrincipalNames.PersonId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.RoleName, SuperOfficeAuthenticationConstants.PrincipalNames.RoleName);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.RoleId, SuperOfficeAuthenticationConstants.PrincipalNames.RoleId);
            ClaimActions.MapJsonKey(SuperOfficeAuthenticationConstants.PrincipalNames.SecondaryGroups, SuperOfficeAuthenticationConstants.PrincipalNames.SecondaryGroups);
        }

        /// <summary>
        /// Gets or sets the Authority to use when making OpenId Connect calls.
        /// </summary>
        public string Authority { get; set; } = string.Empty;

        /// <summary>
        /// Responsible for retrieving, caching, and refreshing the configuration from metadata.
        /// If not provided, then one will be created using the MetadataAddress and Backchannel properties.
        /// </summary>
        public IConfigurationManager<OpenIdConnectConfiguration>? ConfigurationManager { get; set; }

        /// <summary>
        /// Sets the target online environment to either development, stage or production.
        /// </summary>
        public SuperOfficeAuthenticationEnvironment Environment
        {
            get
            {
                return _environment;
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
        /// Gets the sets the URI the middleware uses to obtain the OpenId Connect configuration.
        /// </summary>
        public string MetadataAddress { get; internal set; } = string.Empty;

        /// <summary>
        /// Security token validator. Default is <see cref="System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler"/>.
        /// </summary>
        public JwtSecurityTokenHandler? SecurityTokenHandler { get; set; }

        /// <summary>
        /// Gets or sets the parameters used to validate identity tokens.
        /// </summary>
        /// <remarks>Contains the types and definitions required for validating a token.</remarks>
        public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();

        /// <summary>
        /// Gets or sets a value indicating whether to validate tokens using SuperOffice's public key.
        /// </summary>
        public bool ValidateTokens { get; set; } = true;

        /// <inheritdoc />
        public override void Validate()
        {
            base.Validate();

            if (_environment == SuperOfficeAuthenticationEnvironment.Production
                && !AuthorizationEndpoint.StartsWith("https://", System.StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException("Production environment requires secure endpoints, i.e. begins with 'https://'.");
            }

            if (ConfigurationManager == null)
            {
                throw new InvalidOperationException($"Provide {nameof(Authority)}, {nameof(MetadataAddress)}, "
                + $"or {nameof(ConfigurationManager)} to {nameof(SuperOfficeAuthenticationOptions)}");
            }
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

        /// <summary>
        /// Updates configuration settings.
        /// </summary>
        private void UpdateEndpoints()
        {
            string env = GetEnvironment();

            AuthorizationEndpoint = string.Format(CultureInfo.InvariantCulture,
                SuperOfficeAuthenticationConstants.FormatStrings.AuthorizeEndpoint,
                env);

            TokenEndpoint = string.Format(CultureInfo.InvariantCulture,
                SuperOfficeAuthenticationConstants.FormatStrings.TokenEndpoint,
                env);

            ClaimsIssuer = string.Format(CultureInfo.InvariantCulture,
                SuperOfficeAuthenticationConstants.FormatStrings.ClaimsIssuer,
                env);

            // UserInformationEndpoint will include context identifier after authentication in SuperOfficeAuthenticationHandler.CreateTicketAsync
            UserInformationEndpoint = string.Concat(ClaimsIssuer, SuperOfficeAuthenticationConstants.FormatStrings.UserInfoEndpoint);

            MetadataAddress = string.Format(CultureInfo.InvariantCulture,
                SuperOfficeAuthenticationConstants.FormatStrings.MetadataEndpoint,
                env);

            Authority = string.Format(CultureInfo.InvariantCulture,
                SuperOfficeAuthenticationConstants.FormatStrings.Authority,
                env);
        }
    }
}
