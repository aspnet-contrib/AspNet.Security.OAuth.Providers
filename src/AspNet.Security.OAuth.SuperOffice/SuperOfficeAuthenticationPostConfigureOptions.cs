/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AspNet.Security.OAuth.SuperOffice;

/// <summary>
/// Used to setup defaults for all <see cref="SuperOfficeAuthenticationOptions"/>.
/// </summary>
public class SuperOfficeAuthenticationPostConfigureOptions : IPostConfigureOptions<SuperOfficeAuthenticationOptions>
{
    /// <inheritdoc />
    public void PostConfigure(
        string? name,
        [NotNull] SuperOfficeAuthenticationOptions options)
    {
        if (string.IsNullOrEmpty(options.TokenValidationParameters.ValidAudience) && !string.IsNullOrEmpty(options.ClientId))
        {
            options.TokenValidationParameters.ValidAudience = options.ClientId;
            options.TokenValidationParameters.ValidIssuer = options.ClaimsIssuer;
        }

        // As seen in:
        // github.com/dotnet/aspnetcore/blob/master/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectPostConfigureOptions.cs#L71-L102
        // need this now to successfully instantiate ConfigurationManager below.
        if (options.Backchannel is null)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
#pragma warning restore CA2000 // Dispose objects before losing scope
            options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("SuperOffice OAuth handler");
            options.Backchannel.Timeout = options.BackchannelTimeout;
            options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
        }

        if (options.ConfigurationManager is null)
        {
            if (string.IsNullOrEmpty(options.MetadataAddress))
            {
                throw new InvalidOperationException($"The MetadataAddress must be set on the {nameof(SuperOfficeAuthenticationOptions)} instance.");
            }

            options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                options.MetadataAddress,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever(options.Backchannel))
            {
                AutomaticRefreshInterval = TimeSpan.FromDays(1),
                RefreshInterval = TimeSpan.FromSeconds(30)
            };
        }

        options.SecurityTokenHandler ??= new JsonWebTokenHandler();
    }
}
