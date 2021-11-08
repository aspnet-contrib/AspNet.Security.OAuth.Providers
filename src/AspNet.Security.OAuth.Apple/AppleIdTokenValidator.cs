/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// Represents the base class for validating Apple ID tokens.
    /// </summary>
    public abstract class AppleIdTokenValidator
    {
        /// <summary>
        /// Validates the Apple ID token associated with the specified context as an asynchronous operation.
        /// </summary>
        /// <param name="context">The context to validate the ID token for.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to validate the ID token.
        /// </returns>
        public abstract Task ValidateAsync(AppleValidateIdTokenContext context);
    }
}
