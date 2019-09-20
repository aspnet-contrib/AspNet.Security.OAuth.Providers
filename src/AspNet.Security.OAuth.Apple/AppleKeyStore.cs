/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Threading.Tasks;

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// Represents the base class for a store containing the keys for use with Sign in with Apple.
    /// </summary>
    public abstract class AppleKeyStore
    {
        /// <summary>
        /// Loads the client private key as an asynchronous operation.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation
        /// to get the raw bytes of the private key to use for Sign in with Apple.
        /// </returns>
        public abstract Task<byte[]> LoadPrivateKeyAsync(AppleGenerateClientSecretContext context);

        /// <summary>
        /// Loads the Apple public key as an asynchronous operation.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation
        /// to get the raw bytes of the public key to use for Sign in with Apple.
        /// </returns>
        public abstract Task<byte[]> LoadPublicKeysAsync(AppleValidateIdTokenContext context);
    }
}
