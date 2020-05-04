/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */
using Microsoft.AspNetCore.Authentication;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Contains constants specific to the <see cref="SuperOfficeAuthenticationHandler"/>.
    /// </summary>
    public static class SuperOfficeAuthenticationConstants
    {
        public static class ClaimNames
        {
            /// <summary>
            /// Tenant's user identity.
            /// </summary>
            public const string AssociateId = "http://schemes.superoffice.net/identity/associateid";

            /// <summary>
            /// Current user's company name.
            /// </summary>
            public const string CompanyName = "http://schemes.superoffice.net/identity/company_name";

            /// <summary>
            /// Current user tenant identifier.
            /// </summary>
            public const string ContextIdentifier = "http://schemes.superoffice.net/identity/ctx";

            /// <summary>
            /// Current SuperId user email address.
            /// </summary>
            public const string Email = "http://schemes.superoffice.net/identity/email";

            /// <summary>
            /// Current user first name.
            /// </summary>
            public const string FirstName = "http://schemes.superoffice.net/identity/firstname";

            /// <summary>
            /// SuperOffice identity provider name.
            /// </summary>
            public const string IdentityProvider = "http://schemes.superoffice.net/identity/identityprovider";

            /// <summary>
            /// Current user initials.
            /// </summary>
            public const string Initials = "http://schemes.superoffice.net/identity/initials";

            /// <summary>
            /// Determines if current user is an administrator.
            /// </summary>
            public const string IsAdministrator = "http://schemes.superoffice.net/identity/is_administrator";

            /// <summary>
            /// Current user last name.
            /// </summary>
            public const string LastName = "http://schemes.superoffice.net/identity/lastname";

            /// <summary>
            /// Endpoint of SuperOffice SOAP web services.
            /// </summary>
            public const string NetServerUrl = "http://schemes.superoffice.net/identity/netserver_url";

            /// <summary>
            /// Tenant database serial number.
            /// </summary>
            public const string Serial = "http://schemes.superoffice.net/identity/serial";

            /// <summary>
            /// Current user's tenant primary email address.
            /// </summary>
            public const string PrimaryEmail = "http://schemes.superoffice.net/identity/so_primary_email_address";

            /// <summary>
            /// Identifier used to exchange for a system user ticket.
            /// </summary>
            public const string SystemToken = "http://schemes.superoffice.net/identity/system_token";

            /// <summary>
            /// Credentials token used to access web services.
            /// </summary>
            public const string Ticket = "http://schemes.superoffice.net/identity/ticket";

            /// <summary>
            /// Current user's username.
            /// </summary>
            public const string UserPrincipalName = "http://schemes.superoffice.net/identity/upn";

            /// <summary>
            /// Endpoint of SuperOffice REST web services.
            /// </summary>
            public const string WebApiUrl = "http://schemes.superoffice.net/identity/webapi_url";
        }

        public static class FormatStrings
        {
            /// <summary>
            /// A string format used to poplate oauth authorize endpoint.
            /// </summary>
            public const string AuthorizeEndpoint = "https://{0}.superoffice.com/login/common/oauth/authorize";

            /// <summary>
            /// A string format used to poplate oauth token endpoint.
            /// </summary>
            public const string TokenEndpoint = "https://{0}.superoffice.com/login/common/oauth/tokens";

            /// <summary>
            /// A string format used to obtain user claims.
            /// </summary>
            /// <remarks>The final userinfo url contains protocol, host and tenant</remarks>
            /// <example>https://sod.superoffice.com/Cust12345/api/v1/user/currentPrincipal</example>
            public const string UserInfoEndPoint = "/{0}/api/v1/user/currentPrincipal";

            /// <summary>
            /// Format value used to construct <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
            /// </summary>
            public const string ClaimsIssuer = "https://{0}.superoffice.com";

            /// <summary>
            /// Format value used to construct well-known configuration endpoint.
            /// </summary>
            public const string ConfigurationEndpoint = "https://{0}.superoffice.com/login/.well-known/openid-configuration";
        }

        public static class PrincipalNames
        {
            /// <summary>
            /// Type of user
            /// </summary>
            public const string UserType = "UserType";

            /// <summary>
            /// Associate name (e.g. logon name) for the user
            /// </summary>
            public const string Associate = "Associate";

            /// <summary>
            /// AssociateId for the user.
            /// </summary>
            public const string AssociateId = "AssociateId";

            /// <summary>
            /// Is this associate a person, and not a resource?
            /// </summary>
            public const string IsPerson = "IsPerson";

            /// <summary>
            /// Associate's person id
            /// </summary>
            public const string PersonId = "PersonId";

            /// <summary>
            /// Country id for the user.
            /// </summary>
            public const string CountryId = "CountryId";

            /// <summary>
            /// Country id for the user's home country. This is the default country id when creating new items.
            /// </summary>
            public const string HomeCountryId = "HomeCountryId";

            /// <summary>
            /// Company of the associate's person
            /// </summary>
            public const string ContactId = "ContactId";

            /// <summary>
            /// Associate's group_idx
            /// </summary>
            public const string GroupId = "GroupId";

            /// <summary>
            /// BusinessIdx for the company that the user belongs to.
            /// </summary>
            public const string BusinessId = "BusinessId";

            /// <summary>
            /// CategoryIdx of the company that the user belongs to.
            /// </summary>
            public const string CategoryId = "CategoryId";

            /// <summary>
            /// Owner (AssociateId) of the company that the user belongs to.
            /// </summary>
            public const string ContactOwner = "ContactOwner";

            /// <summary>
            /// Id of the users role
            /// </summary>
            public const string RoleId = "RoleId";

            /// <summary>
            /// Name of the users role
            /// </summary>
            public const string RoleName = "RoleName";

            /// <summary>
            /// Descrtiption (e.g. tooltip) for the users role
            /// </summary>
            public const string RoleDescription = "RoleDescription";

            /// <summary>
            /// Type of user.
            /// </summary>
            public const string RoleType = "RoleType";

            /// <summary>
            ///  License granted to the site and user.
            /// </summary>
            public const string Licenses = "Licenses";

            /// <summary>
            /// The Person full name if the associate is a person. Use IsPerson to check
            /// </summary>
            public const string FullName = "FullName";

            /// <summary>
            /// The Person e-mail address if the associate is a person. Use IsPerson to check
            /// </summary>
            public const string EMailAddress = "EMailAddress";

            /// <summary>
            /// Functional rights for the user. This array is sorted.
            /// </summary>
            public const string FunctionRights = "FunctionRights";

            /// <summary>
            /// Primary key in eJournal's old user table.
            /// </summary>
            public const string EjUserId = "EjUserId";

            /// <summary>
            /// eJournal user access level
            /// </summary>
            public const string EjAccessLevel = "EjAccessLevel";

            /// <summary>
            /// eJournal user status
            /// </summary>
            public const string EjUserStatus = "EjUserStatus";

            /// <summary>
            /// The credentials used for authenticating this user
            /// </summary>
            public const string ProvidedCredentials = "ProvidedCredentials";

            /// <summary>
            /// Secondary user groups
            /// </summary>
            public const string SecondaryGroups = "SecondaryGroups";

            /// <summary>
            /// Name of the tenant context identifier
            /// </summary>
            public const string ContextIdentifier = "ContextIdentifier";

            /// <summary>
            /// Name of the database context
            /// </summary>
            public const string DatabaseContextIdentifier = "DatabaseContextIdentifier";
        }
    }
}
