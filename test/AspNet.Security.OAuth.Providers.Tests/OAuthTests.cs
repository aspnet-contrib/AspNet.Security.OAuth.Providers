/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JustEat.HttpClientInterception;
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

        protected void ConfigureTokenEndpoint(string uriString, object content = null)
        {
            if (content == null)
            {
                content = new
                {
                    access_token = "secret-access-token",
                    token_type = "access",
                    refresh_token = "secret-refresh-token",
                    expires_in = "300",
                };
            }

            var builder = new HttpRequestInterceptionBuilder()
                .Requests().ForPost().ForUrl(uriString)
                .Responds().WithJsonContent(content);

            Interceptor.Register(builder);
        }

        protected void ConfigureUserEndpoint(string uriString, object content)
        {
            var builder = new HttpRequestInterceptionBuilder()
                .Requests().ForGet().ForUrl(uriString)
                .Responds().WithJsonContent(content);

            Interceptor.Register(builder);
        }
    }
}
