/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Used to setup defaults for all <see cref="SuperOfficeAuthenticationOptions"/>.
    /// </summary>
    public class SuperOfficeAuthenticationPostConfigureOptions : IPostConfigureOptions<SuperOfficeAuthenticationOptions>
    {
        public void PostConfigure(
            [NotNull] string name,
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
            if (options.Backchannel == null)
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
                options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
#pragma warning restore CA2000 // Dispose objects before losing scope
                options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("SuperOffice OpenIdConnect handler");
                options.Backchannel.Timeout = options.BackchannelTimeout;
                options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
            }

            if (options.ConfigurationManager == null)
            {
                if (!(string.IsNullOrEmpty(options.MetadataAddress) && string.IsNullOrEmpty(options.Authority)))
                {
                    if (string.IsNullOrEmpty(options.MetadataAddress) && !string.IsNullOrEmpty(options.Authority))
                    {
                        options.MetadataAddress = options.Authority;
                        if (!options.MetadataAddress.EndsWith("/", StringComparison.Ordinal))
                        {
                            options.MetadataAddress += "/";
                        }

                        options.MetadataAddress += ".well-known/openid-configuration";
                    }

                    if (!options.MetadataAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("The MetadataAddress or Authority must use HTTPS unless disabled for development by setting RequireHttpsMetadata=false.");
                    }

                    options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                        options.MetadataAddress,
                        new OpenIdConnectConfigurationRetriever(),
                        new HttpDocumentRetriever(options.Backchannel))
                        {
                            RefreshInterval = TimeSpan.FromSeconds(30),
                            AutomaticRefreshInterval = TimeSpan.FromDays(1)
                        };
                }
            }
        }
    }
}
