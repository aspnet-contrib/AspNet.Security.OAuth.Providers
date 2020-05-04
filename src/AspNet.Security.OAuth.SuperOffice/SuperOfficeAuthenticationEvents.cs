/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Default <see cref="SuperOfficeAuthenticationEvents"/> implementation.
    /// </summary>
    public class SuperOfficeAuthenticationEvents : OAuthEvents
    {
        /// <summary>
        /// Gets or sets the delegate that is invoked when the <see cref="ValidateIdToken"/> method is invoked.
        /// </summary>
        public Func<SuperOfficeValidateIdTokenContext, Task> OnValidateIdToken { get; set; } = async context =>
        {
            var validator = context.HttpContext.RequestServices.GetRequiredService<SuperOfficeIdTokenValidator>();
            await validator.ValidateAsync(context);
        };

        /// <summary>
        /// Invoked whenever the ID token needs to be validated.
        /// </summary>
        /// <param name="context">Contains information about the ID token to validate.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the completed operation.
        /// </returns>
        public virtual async Task ValidateIdToken(SuperOfficeValidateIdTokenContext context) => await OnValidateIdToken(context);
    }
}
