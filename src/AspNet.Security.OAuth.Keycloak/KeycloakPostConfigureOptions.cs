// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
// for more information concerning the license and the contributors participating to this project.

using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Keycloak
{
    /// <summary>
    /// A class used to setup defaults for all <see cref="KeycloakAuthenticationOptions"/>.
    /// </summary>
    public class KeycloakPostConfigureOptions : IPostConfigureOptions<KeycloakAuthenticationOptions>
    {
        public void PostConfigure([NotNull] string name, [NotNull] KeycloakAuthenticationOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.Domain))
            {
                options.AuthorizationEndpoint = CreateUrl(options.Domain, KeycloakAuthenticationDefaults.AuthorizationEndpoint);
                options.TokenEndpoint = CreateUrl(options.Domain, KeycloakAuthenticationDefaults.TokenEndpoint);
                options.UserInformationEndpoint = CreateUrl(options.Domain, KeycloakAuthenticationDefaults.UserInformationEndpoint);
            }
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
