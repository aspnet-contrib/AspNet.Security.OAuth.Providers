/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Apple;

/// <summary>
/// Represents the base class for a client secret generator for Sign in with Apple.
/// </summary>
public abstract class AppleClientSecretGenerator
{
    /// <summary>
    /// Generates a client secret for Sign in with Apple as an asynchronous operation.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation
    /// to generate a client secret for Sign in with Apple.
    /// </returns>
    public abstract Task<string> GenerateAsync(AppleGenerateClientSecretContext context);
}
