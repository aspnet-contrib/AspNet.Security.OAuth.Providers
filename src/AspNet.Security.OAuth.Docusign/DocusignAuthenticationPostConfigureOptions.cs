/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Docusign;

/// <summary>
/// Used to setup defaults for all <see cref="DocusignAuthenticationOptions"/>.
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
                options.AuthorizationEndpoint = DocusignAuthenticationDefaults.ProductionAuthorizationEndpoint;
                options.TokenEndpoint = DocusignAuthenticationDefaults.ProductionTokenEndpoint;
                options.UserInformationEndpoint = DocusignAuthenticationDefaults.ProductionUserInformationEndpoint;
                break;
            case DocusignAuthenticationEnvironment.Development:
                options.AuthorizationEndpoint = DocusignAuthenticationDefaults.DevelopmentAuthorizationEndpoint;
                options.TokenEndpoint = DocusignAuthenticationDefaults.DevelopmentTokenEndpoint;
                options.UserInformationEndpoint = DocusignAuthenticationDefaults.DevelopmentUserInformationEndpoint;
                break;
            default:
                throw new InvalidOperationException($"The {nameof(Environment)} is not supported.");
        }
    }
}
