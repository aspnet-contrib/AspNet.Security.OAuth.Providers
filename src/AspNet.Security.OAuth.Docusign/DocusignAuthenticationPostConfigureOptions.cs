/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Docusign;

/// <summary>
/// Used to configure <see cref="DocusignAuthenticationOptions"/> instances.
/// </summary>
public sealed class DocusignAuthenticationPostConfigureOptions : IPostConfigureOptions<DocusignAuthenticationOptions>
{
    /// <inheritdoc />
    public void PostConfigure(
        string? name,
        [NotNull] DocusignAuthenticationOptions options)
    {
        ConfigureEndpoints(options);
    }

    private static void ConfigureEndpoints(DocusignAuthenticationOptions options)
    {
        switch (options.Environment)
        {
            case DocusignAuthenticationEnvironment.Production:
                options.AuthorizationEndpoint = CreateUrl(DocusignAuthenticationDefaults.ProductionDomain, DocusignAuthenticationDefaults.AuthorizationPath);
                options.TokenEndpoint = CreateUrl(DocusignAuthenticationDefaults.ProductionDomain, DocusignAuthenticationDefaults.TokenPath);
                options.UserInformationEndpoint = CreateUrl(DocusignAuthenticationDefaults.ProductionDomain, DocusignAuthenticationDefaults.UserInformationPath);
                break;
            case DocusignAuthenticationEnvironment.Development:
                options.AuthorizationEndpoint = CreateUrl(DocusignAuthenticationDefaults.DevelopmentDomain, DocusignAuthenticationDefaults.AuthorizationPath);
                options.TokenEndpoint = CreateUrl(DocusignAuthenticationDefaults.DevelopmentDomain, DocusignAuthenticationDefaults.TokenPath);
                options.UserInformationEndpoint = CreateUrl(DocusignAuthenticationDefaults.DevelopmentDomain, DocusignAuthenticationDefaults.UserInformationPath);
                break;
            default:
                throw new InvalidOperationException($"The {nameof(Environment)} is not supported.");
        }
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
