/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;

namespace AspNet.Security.OAuth.MailRu
{
    /// <summary>
    /// Default values used by the MailRu authentication middleware.
    /// </summary>
    public static class MailRuAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.AuthenticationScheme"/>.
        /// </summary>
        public const string AuthenticationScheme = "MailRu";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Mail.ru";

        /// <summary>
        /// Default value for <see cref="AuthenticationOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "MailRu";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-mailru";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://connect.mail.ru/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://connect.mail.ru/oauth/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://www.appsmail.ru/platform/api";
    }
}