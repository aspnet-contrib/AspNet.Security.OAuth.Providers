/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.GitHub
{
    /// <summary>
    /// A class used to setup defaults for all <see cref="GitHubAuthenticationOptions"/>.
    /// </summary>
    public class GitHubPostConfigureOptions : IPostConfigureOptions<GitHubAuthenticationOptions>
    {
        /// <inheritdoc/>
        public void PostConfigure(
            [NotNull] string name,
            [NotNull] GitHubAuthenticationOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.EnterpriseDomain))
            {
                options.AuthorizationEndpoint = CreateUrl(options.EnterpriseDomain, GitHubAuthenticationDefaults.AuthorizationEndpointPath);
                options.TokenEndpoint = CreateUrl(options.EnterpriseDomain, GitHubAuthenticationDefaults.TokenEndpointPath);
                options.UserEmailsEndpoint = CreateUrl(options.EnterpriseDomain, GitHubAuthenticationDefaults.UserEmailsEndpointPath, "api");
                options.UserInformationEndpoint = CreateUrl(options.EnterpriseDomain, GitHubAuthenticationDefaults.UserInformationEndpointPath, "api");
            }
        }

        private static string CreateUrl(string domain, string path, string? subdomain = null)
        {
            // Enforce use of HTTPS
            var builder = new UriBuilder(domain)
            {
                Path = path,
                Port = -1,
                Scheme = "https",
            };

            if (!string.IsNullOrEmpty(subdomain))
            {
                builder.Host = subdomain + "." + builder.Host;
            }

            return builder.Uri.ToString();
        }
    }
}
