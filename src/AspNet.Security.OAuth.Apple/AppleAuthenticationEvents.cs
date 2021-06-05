/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// Default <see cref="AppleAuthenticationEvents"/> implementation.
    /// </summary>
    public class AppleAuthenticationEvents : OAuthEvents
    {
        /// <summary>
        /// Gets or sets the delegate that is invoked when the <see cref="GenerateClientSecret"/> method is invoked.
        /// </summary>
        public Func<AppleGenerateClientSecretContext, Task> OnGenerateClientSecret { get; set; } = async context =>
        {
            context.Options.ClientSecret = await context.Options.ClientSecretGenerator.GenerateAsync(context);
        };

        /// <summary>
        /// Gets or sets the delegate that is invoked when the <see cref="ValidateIdToken"/> method is invoked.
        /// </summary>
        public Func<AppleValidateIdTokenContext, Task> OnValidateIdToken { get; set; } = async context =>
        {
            await context.Options.TokenValidator.ValidateAsync(context);
        };

        /// <summary>
        /// Invoked whenever the client secret needs to be generated.
        /// </summary>
        /// <param name="context">Contains information about the current request.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the completed operation.
        /// </returns>
        public virtual async Task GenerateClientSecret([NotNull] AppleGenerateClientSecretContext context) =>
            await OnGenerateClientSecret(context);

        /// <summary>
        /// Invoked whenever the ID token needs to be validated.
        /// </summary>
        /// <param name="context">Contains information about the ID token to validate.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the completed operation.
        /// </returns>
        public virtual async Task ValidateIdToken([NotNull] AppleValidateIdTokenContext context) =>
            await OnValidateIdToken(context);
    }
}
