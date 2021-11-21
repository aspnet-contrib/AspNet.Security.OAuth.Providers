/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;
using static AspNet.Security.OAuth.Zendesk.ZendeskAuthenticationDefaults;

namespace AspNet.Security.OAuth.Zendesk;

/// <summary>
/// A class used to setup defaults for all <see cref="ZendeskAuthenticationOptions"/>.
/// </summary>
public class ZendeskPostConfigureOptions : IPostConfigureOptions<ZendeskAuthenticationOptions>
{
    /// <inheritdoc/>
    public void PostConfigure(
        [NotNull] string name,
        [NotNull] ZendeskAuthenticationOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Domain))
        {
            throw new ArgumentException($"No Zendesk domain configured", nameof(options));
        }

        options.AuthorizationEndpoint = CreateUrl(options.Domain, AuthorizationEndpointPath);
        options.TokenEndpoint = CreateUrl(options.Domain, TokenEndpointPath);
        options.UserInformationEndpoint = CreateUrl(options.Domain, UserInformationEndpointPath);
    }

    private static string CreateUrl(string domain, string path)
    {
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
