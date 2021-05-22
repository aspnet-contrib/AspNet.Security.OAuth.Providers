/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Bitrix24
{
    public class Bitrix24PostConfigureOptions : IPostConfigureOptions<Bitrix24AuthenticationOptions>
    {
        /// <inheritdoc/>
        public void PostConfigure(
            [NotNull] string name,
            [NotNull] Bitrix24AuthenticationOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Domain))
            {
                throw new ArgumentException("No Bitrix24 domain configured.", nameof(options));
            }

            options.AuthorizationEndpoint = CreateUrl(options.Domain, Bitrix24AuthenticationDefaults.AuthorizationEndpointPath);
            options.TokenEndpoint = CreateUrl(options.Domain, Bitrix24AuthenticationDefaults.TokenEndpointPath);
            options.UserInformationEndpoint = CreateUrl(options.Domain, Bitrix24AuthenticationDefaults.UserInformationEndpointPath);
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
