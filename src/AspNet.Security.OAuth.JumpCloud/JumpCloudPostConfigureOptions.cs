/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.JumpCloud;

/// <summary>
/// A class used to setup defaults for all <see cref="JumpCloudAuthenticationOptions"/>.
/// </summary>
public class JumpCloudPostConfigureOptions : IPostConfigureOptions<JumpCloudAuthenticationOptions>
{
    /// <inheritdoc/>
    public void PostConfigure(
        string? name,
        [NotNull] JumpCloudAuthenticationOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Domain))
        {
            throw new ArgumentException("No JumpCloud domain configured.", nameof(options));
        }

        options.AuthorizationEndpoint = CreateUrl(options.Domain, JumpCloudAuthenticationDefaults.AuthorizationEndpointPathFormat);
        options.TokenEndpoint = CreateUrl(options.Domain, JumpCloudAuthenticationDefaults.TokenEndpointPathFormat);
        options.UserInformationEndpoint = CreateUrl(options.Domain, JumpCloudAuthenticationDefaults.UserInformationEndpointPathFormat);
    }

    private static string CreateUrl(string domain, string pathFormat, params object[] args)
    {
        var path = string.Format(CultureInfo.InvariantCulture, pathFormat, args);

        // Enforce use of HTTPS
        var builder = new UriBuilder(domain)
        {
            Path = path,
            Port = -1,
            Scheme = Uri.UriSchemeHttps,
        };

        return builder.Uri.ToString();
    }
}
