/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Moodle;

/// <summary>
/// A class used to setup defaults for all <see cref="MoodleAuthenticationOptions"/>.
/// </summary>
public class MoodlePostConfigureOptions : IPostConfigureOptions<MoodleAuthenticationOptions>
{
    /// <inheritdoc/>
    public void PostConfigure(
        [NotNull] string name,
        [NotNull] MoodleAuthenticationOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Domain))
        {
            throw new ArgumentException("No Moodle domain configured.", nameof(options));
        }

        options.AuthorizationEndpoint = CreateUrl(options.Domain, MoodleAuthenticationDefaults.AuthorizationEndpointPath);
        options.TokenEndpoint = CreateUrl(options.Domain, MoodleAuthenticationDefaults.TokenEndpointPath);
        options.UserInformationEndpoint = CreateUrl(options.Domain, MoodleAuthenticationDefaults.UserInformationEndpointPath);
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
