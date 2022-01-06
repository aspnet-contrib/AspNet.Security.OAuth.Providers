/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Okta;

/// <summary>
/// A class used to setup defaults for all <see cref="OktaAuthenticationOptions"/>.
/// </summary>
public class OktaPostConfigureOptions : IPostConfigureOptions<OktaAuthenticationOptions>
{
    /// <inheritdoc/>
    public void PostConfigure(
        [NotNull] string name,
        [NotNull] OktaAuthenticationOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Domain))
        {
            throw new ArgumentException("No Okta domain configured.", nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.AuthorizationServer))
        {
            throw new ArgumentException("No Okta authorization server configured.", nameof(options));
        }

        options.AuthorizationEndpoint = CreateUrl(options.Domain, OktaAuthenticationDefaults.AuthorizationEndpointPathFormat, options.AuthorizationServer);
        options.TokenEndpoint = CreateUrl(options.Domain, OktaAuthenticationDefaults.TokenEndpointPathFormat, options.AuthorizationServer);
        options.UserInformationEndpoint = CreateUrl(options.Domain, OktaAuthenticationDefaults.UserInformationEndpointPathFormat, options.AuthorizationServer);
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
