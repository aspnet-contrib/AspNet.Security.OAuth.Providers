/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Represents the base class for loading configuration and obtaining keys for use with Sign in with SuperOffice.
    /// </summary>
    public abstract class SuperOfficeAuthenticationConfigurationManager
    {
        public SuperOfficeAuthenticationConfigurationManager()
        {
        }

        /// <summary>
        /// Loads the SuperOffice public key as an asynchronous operation.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation
        /// to get the json string of the public key to use for Sign in with SuperOffice.
        /// </returns>
        public abstract Task<JsonWebKeySet> GetPublicKeySetAsync([NotNull] SuperOfficeValidateIdTokenContext context);
    }
}
