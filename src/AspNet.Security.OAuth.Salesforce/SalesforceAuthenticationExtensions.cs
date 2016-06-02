/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Salesforce;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods to add Salesforce authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class SalesforceAuthenticationExtensions
    {
        /// <summary>
        /// Adds the <see cref="SalesforceAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Salesforce authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="SalesforceAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseSalesforceAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] SalesforceAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<SalesforceAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="SalesforceAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Salesforce authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="SalesforceAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseSalesforceAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<SalesforceAuthenticationOptions> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = new SalesforceAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<SalesforceAuthenticationMiddleware>(Options.Create(options));
        }
    }
}
