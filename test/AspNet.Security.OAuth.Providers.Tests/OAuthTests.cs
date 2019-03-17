/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth
{
    /// <summary>
    /// The base class for integration tests for OAuth-based authentication providers.
    /// </summary>
    /// <typeparam name="TOptions">The options type for the authentication provider being tested.</typeparam>
    public abstract class OAuthTests<TOptions> : AuthenticationTests<TOptions>
        where TOptions : OAuthOptions
    {
        protected override void ConfigureDefaults(AuthenticationBuilder builder, TOptions options)
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            base.ConfigureDefaults(builder, options);
        }
    }
}
