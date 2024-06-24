/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Zoho;

/// <summary>
/// Used to configure <see cref="ZohoAuthenticationOptions"/> instances.
/// </summary>
public sealed class ZohoAuthenticationPostConfigureOptions : IPostConfigureOptions<ZohoAuthenticationOptions>
{
    /// <inheritdoc />
    public void PostConfigure(
        string? name,
        [NotNull] ZohoAuthenticationOptions options)
    {
        ConfigureEndpoints(options);
    }

    private static void ConfigureEndpoints(ZohoAuthenticationOptions options)
    {
        var domain = GetDomain(options.Region);

        if (string.IsNullOrEmpty(options.AuthorizationEndpoint))
        {
            options.AuthorizationEndpoint = CreateUrl(domain, ZohoAuthenticationDefaults.AuthorizationPath);
        }

        if (string.IsNullOrEmpty(options.TokenEndpoint))
        {
            options.TokenEndpoint = CreateUrl(domain, ZohoAuthenticationDefaults.TokenPath);
        }

        if (string.IsNullOrEmpty(options.UserInformationEndpoint))
        {
            options.UserInformationEndpoint = CreateUrl(domain, ZohoAuthenticationDefaults.UserInformationPath);
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

    private static string GetDomain(ZohoAuthenticationRegion region)
    {
        return region switch
        {
            ZohoAuthenticationRegion.Australia => "accounts.zoho.com.au",
            ZohoAuthenticationRegion.Canada => "accounts.zohocloud.ca",
            ZohoAuthenticationRegion.Europe => "accounts.zoho.eu",
            ZohoAuthenticationRegion.Global => "accounts.zoho.com",
            ZohoAuthenticationRegion.India => "accounts.zoho.in",
            ZohoAuthenticationRegion.Japan => "accounts.zoho.jp",
            ZohoAuthenticationRegion.SaudiArabia => "accounts.zoho.sa",
            _ => throw new InvalidOperationException($"The {nameof(ZohoAuthenticationRegion)} is not supported."),
        };
    }
}
