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
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using static AspNet.Security.OAuth.SuperOffice.SuperOfficeAuthenticationConstants;

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

            ClaimActions.MapJsonKey(ClaimTypes.Name, PrincipalNames.FullName);
            ClaimActions.MapJsonKey(ClaimTypes.Email, PrincipalNames.EmailAddress);

            ClaimActions.MapJsonKey(ClaimNames.AssociateId, PrincipalNames.AssociateId);
            ClaimActions.MapJsonKey(ClaimNames.Email, PrincipalNames.EmailAddress);
            ClaimActions.MapJsonKey(ClaimNames.UserPrincipalName, PrincipalNames.Associate);

            ClaimActions.MapJsonKey(PrincipalNames.BusinessId, PrincipalNames.BusinessId);
            ClaimActions.MapJsonKey(PrincipalNames.CategoryId, PrincipalNames.CategoryId);
            ClaimActions.MapJsonKey(PrincipalNames.ContactId, PrincipalNames.ContactId);
            ClaimActions.MapJsonKey(PrincipalNames.ContextIdentifier, PrincipalNames.DatabaseContextIdentifier);
            ClaimActions.MapJsonKey(PrincipalNames.CountryId, PrincipalNames.CountryId);
            ClaimActions.MapJsonKey(PrincipalNames.GroupId, PrincipalNames.GroupId);
            ClaimActions.MapJsonKey(PrincipalNames.HomeCountryId, PrincipalNames.HomeCountryId);
            ClaimActions.MapJsonKey(PrincipalNames.PersonId, PrincipalNames.PersonId);
            ClaimActions.MapJsonKey(PrincipalNames.RoleName, PrincipalNames.RoleName);
            ClaimActions.MapJsonKey(PrincipalNames.RoleId, PrincipalNames.RoleId);
            ClaimActions.MapJsonKey(PrincipalNames.SecondaryGroups, PrincipalNames.SecondaryGroups);

            // Add a custom claim action to map FunctionRights to claims
            ClaimActions.Add(new SuperOfficeFunctionalRightsClaimAction(this));
        }

        /// <summary>
        /// Gets or sets the Authority to use when making OpenId Connect calls.
        /// </summary>
        public string Authority { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the configuration manager responsible for retrieving, caching, and refreshing the
        /// OpenId configuration from metadata. If not provided, then one will be created using the <see cref="MetadataAddress"/>
        /// and <see cref="RemoteAuthenticationOptions.Backchannel"/> properties.
        /// </summary>
        public IConfigurationManager<OpenIdConnectConfiguration>? ConfigurationManager { get; set; }

        /// <summary>
        /// Gets or sets the target online environment to either development, stage or production.
        /// The default value is <see cref="SuperOfficeAuthenticationEnvironment.Development"/>.
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
        /// Gets or sets a value indicating whether user claims will include SuperOffice function rights.
        /// </summary>
        public bool IncludeFunctionalRightsAsClaims { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user claims will include the SuperOffice <c>id_token</c> claims.
        /// </summary>
        public bool IncludeIdTokenAsClaims { get; set; }

        /// <summary>
        /// Gets the URI the middleware uses to obtain the OpenId Connect configuration.
        /// </summary>
        public string MetadataAddress { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the security token validator to use.
        /// </summary>
        public JsonWebTokenHandler? SecurityTokenHandler { get; set; }

        /// <summary>
        /// Gets or sets the parameters used to validate identity tokens.
        /// </summary>
        /// <remarks>Contains the types and definitions required for validating a token.</remarks>
        public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();

        /// <inheritdoc />
        public override void Validate()
        {
            base.Validate();

            if (_environment == SuperOfficeAuthenticationEnvironment.Production
                && !AuthorizationEndpoint.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException("Production environment requires secure endpoints, i.e. begins with 'https://'.");
            }

            if (ConfigurationManager == null)
            {
                throw new InvalidOperationException($"Provide {nameof(Authority)}, {nameof(MetadataAddress)}, or {nameof(ConfigurationManager)} to {nameof(SuperOfficeAuthenticationOptions)}.");
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
                SuperOfficeAuthenticationEnvironment.Stage => "qaonline",
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
                FormatStrings.AuthorizeEndpoint,
                env);

            TokenEndpoint = string.Format(CultureInfo.InvariantCulture,
                FormatStrings.TokenEndpoint,
                env);

            ClaimsIssuer = string.Format(CultureInfo.InvariantCulture,
                FormatStrings.ClaimsIssuer,
                env);

            // UserInformationEndpoint will include context identifier after authentication in SuperOfficeAuthenticationHandler.CreateTicketAsync
            UserInformationEndpoint = string.Concat(ClaimsIssuer, FormatStrings.UserInfoEndpoint);

            MetadataAddress = string.Format(CultureInfo.InvariantCulture,
                FormatStrings.MetadataEndpoint,
                env);

            Authority = string.Format(CultureInfo.InvariantCulture,
                FormatStrings.Authority,
                env);
        }
    }
}
