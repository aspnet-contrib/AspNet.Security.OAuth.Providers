/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Okta
{
    /// <summary>
    /// A class used to setup defaults for all <see cref="OktaAuthenticationOptions"/>.
    /// </summary>
    public class OktaPostConfigureOptions : IPostConfigureOptions<OktaAuthenticationOptions>
    {
        /// <inheritdoc/>
        public void PostConfigure(string name, OktaAuthenticationOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Domain))
            {
                throw new ArgumentException("No Okta domain configured.", nameof(options));
            }

            options.AuthorizationEndpoint = CreateUrl(options.Domain, OktaAuthenticationDefaults.AuthorizationEndpointPath);
            options.TokenEndpoint = CreateUrl(options.Domain, OktaAuthenticationDefaults.TokenEndpointPath);
            options.UserInformationEndpoint = CreateUrl(options.Domain, OktaAuthenticationDefaults.UserInformationEndpointPath);
        }

        private static string CreateUrl(string domain, string path)
        {
            // Enforce use of HTTPS
            var builder = new UriBuilder(domain)
            {
                Path = path,
                Port = -1,
                Scheme = "https",
            };

            return builder.Uri.ToString();
        }
    }
}
