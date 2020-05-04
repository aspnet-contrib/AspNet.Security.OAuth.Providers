/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Contains information about the ID token to validate.
    /// </summary>
    public class SuperOfficeValidateIdTokenContext : BaseContext<SuperOfficeAuthenticationOptions>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SuperOfficeValidateIdTokenContext"/> class.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="scheme">The authentication scheme.</param>
        /// <param name="options">The authentication options associated with the scheme.</param>
        /// <param name="idToken">The SuperOffice ID token for the user to validate.</param>
        public SuperOfficeValidateIdTokenContext(
            [NotNull] HttpContext context,
            [NotNull] AuthenticationScheme scheme,
            [NotNull] SuperOfficeAuthenticationOptions options,
            [NotNull] string idToken)
            : base(context, scheme, options)
        {
            IdToken = idToken;
        }

        /// <summary>
        /// Gets the SuperOffice ID token.
        /// </summary>
        public string IdToken { get; }
    }
}
