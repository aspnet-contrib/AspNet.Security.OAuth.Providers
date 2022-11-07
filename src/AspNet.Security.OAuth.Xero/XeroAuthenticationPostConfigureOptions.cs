/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AspNet.Security.OAuth.Xero;

/// <summary>
/// Used to setup defaults for all <see cref="XeroAuthenticationOptions"/>.
/// </summary>
public class XeroAuthenticationPostConfigureOptions : IPostConfigureOptions<XeroAuthenticationOptions>
{
    /// <inheritdoc />
    public void PostConfigure(
        string? name,
        [NotNull] XeroAuthenticationOptions options)
    {
        if (string.IsNullOrEmpty(options.TokenValidationParameters.ValidAudience) && !string.IsNullOrEmpty(options.ClientId))
        {
            options.TokenValidationParameters.ValidateAudience = true;
            options.TokenValidationParameters.ValidAudience = options.ClientId;

            options.TokenValidationParameters.ValidateIssuer = true;
            options.TokenValidationParameters.ValidIssuer = options.ClaimsIssuer;

            options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        }

        // As seen in:
        // github.com/dotnet/aspnetcore/blob/master/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectPostConfigureOptions.cs#L71-L102
        // need this now to successfully instantiate ConfigurationManager below.
        if (options.Backchannel == null)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
#pragma warning restore CA2000 // Dispose objects before losing scope
            options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Xero OAuth handler");
            options.Backchannel.Timeout = options.BackchannelTimeout;
            options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
        }

        if (options.ConfigurationManager == null)
        {
            options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                options.MetadataAddress,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever(options.Backchannel))
            {
                AutomaticRefreshInterval = TimeSpan.FromDays(1),
                RefreshInterval = TimeSpan.FromSeconds(30)
            };
        }

        if (options.SecurityTokenHandler == null)
        {
            options.SecurityTokenHandler = new JsonWebTokenHandler();
        }
    }
}
