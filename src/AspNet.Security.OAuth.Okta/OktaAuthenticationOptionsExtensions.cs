/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Globalization;
using JetBrains.Annotations;

namespace AspNet.Security.OAuth.Okta
{
    /// <summary>
    /// Extension methods to configure Okta authentication capabilities for an HTTP application pipeline.
    /// </summary>
    public static class OktaAuthenticationOptionsExtensions
    {
        /// <summary>
        /// Configures the application to use a specified domain for the Okta provider.
        /// </summary>
        /// <param name="options">The Okta authentication options to configure.</param>
        /// <param name="domain">The domain to use for Okta authentication.</param>
        /// <returns>
        /// The value of the <paramref name="options"/> argument.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domain"/> is <see langword="null"/> or white space.
        /// </exception>
        public static OktaAuthenticationOptions UseDomain(
            [NotNull] this OktaAuthenticationOptions options,
            [NotNull] string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                throw new ArgumentException("No Okta domain name specified.", nameof(domain));
            }

            options.AuthorizationEndpoint = string.Format(CultureInfo.InvariantCulture, OktaAuthenticationDefaults.AuthorizationEndpointFormat, domain);
            options.TokenEndpoint = string.Format(CultureInfo.InvariantCulture, OktaAuthenticationDefaults.TokenEndpointFormat, domain);
            options.UserInformationEndpoint = string.Format(CultureInfo.InvariantCulture, OktaAuthenticationDefaults.UserInformationEndpointFormat, domain);

            return options;
        }
    }
}
