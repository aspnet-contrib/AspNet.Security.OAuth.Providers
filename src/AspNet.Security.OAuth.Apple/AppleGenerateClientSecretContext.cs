/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// Contains information about the current request.
    /// </summary>
    public class AppleGenerateClientSecretContext : BaseContext<AppleAuthenticationOptions>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="AppleGenerateClientSecretContext"/> class.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="scheme">The authentication scheme.</param>
        /// <param name="options">The authentication options associated with the scheme.</param>
        public AppleGenerateClientSecretContext(HttpContext context, AuthenticationScheme scheme, AppleAuthenticationOptions options)
            : base(context, scheme, options)
        {
        }
    }
}
