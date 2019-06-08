/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Threading.Tasks;

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// Represents the base class for a client secret generator for Sign In with Apple.
    /// </summary>
    public abstract class AppleClientSecretGenerator
    {
        /// <summary>
        /// Gets or sets the period of time after which generated client secrets expire.
        /// </summary>
        /// <remarks>
        /// The default client secret lifetime is six months.
        /// </remarks>
        public TimeSpan ExpiresAfter { get; set; } = TimeSpan.FromSeconds(15777000); // 6 months in seconds

        /// <summary>
        /// Generates a client secret for Sign In with Apple as an asynchronous operation.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation
        /// to generate a client secret for Sign In with Apple.
        /// </returns>
        public abstract Task<string> GenerateAsync(AppleGenerateClientSecretContext context);
    }
}
