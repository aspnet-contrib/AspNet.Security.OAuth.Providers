// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
// for more information concerning the license and the contributors participating to this project.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Keycloak;

/// <summary>
/// A class used to setup defaults for all <see cref="KeycloakAuthenticationOptions"/>.
/// </summary>
public class KeycloakPostConfigureOptions : IPostConfigureOptions<KeycloakAuthenticationOptions>
{
    private static readonly Version NoAuthPrefixVersion = new(18, 0);

    public void PostConfigure([NotNull] string name, [NotNull] KeycloakAuthenticationOptions options)
    {
        if ((!string.IsNullOrWhiteSpace(options.Domain) || options.BaseAddress is not null) &&
            !string.IsNullOrWhiteSpace(options.Realm))
        {
            options.AuthorizationEndpoint = CreateUrl(options, KeycloakAuthenticationDefaults.AuthorizationEndpoint);
            options.TokenEndpoint = CreateUrl(options, KeycloakAuthenticationDefaults.TokenEndpoint);
            options.UserInformationEndpoint = CreateUrl(options, KeycloakAuthenticationDefaults.UserInformationEndpoint);
        }
    }

    private static string CreateUrl(KeycloakAuthenticationOptions options, string resource)
    {
        var builder = options.BaseAddress is not null ?
            new UriBuilder(options.BaseAddress) :
            new UriBuilder(options.Domain!);

        // Enforce use of HTTPS if only the domain is specified
        if (options.BaseAddress is null)
        {
            builder.Port = -1;
            builder.Scheme = Uri.UriSchemeHttps;
        }

        var pathBase = new PathString("/");

        if (options.Version is null || options.Version < NoAuthPrefixVersion)
        {
            pathBase = pathBase.Add("/auth");
        }

        builder.Path = pathBase
            .Add("/realms")
            .Add("/" + options.Realm!.Trim('/'))
            .Add(resource);

        return builder.Uri.ToString();
    }
}
