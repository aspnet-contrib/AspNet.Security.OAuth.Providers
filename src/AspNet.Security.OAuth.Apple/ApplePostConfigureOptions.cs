/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Apple.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Apple;

/// <summary>
/// A class used to setup defaults for all <see cref="AppleAuthenticationOptions"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ApplePostConfigureOptions"/> class.
/// </remarks>
/// <param name="cache">The <see cref="IMemoryCache"/> to use.</param>
/// <param name="timeProvider">The <see cref="TimeProvider"/> to use.</param>
/// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use.</param>
public class ApplePostConfigureOptions(
    IMemoryCache cache,
    TimeProvider timeProvider,
    ILoggerFactory loggerFactory) : IPostConfigureOptions<AppleAuthenticationOptions>
{
    private readonly ILoggerFactory _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;

    /// <inheritdoc/>
    public void PostConfigure(
        string? name,
        [NotNull] AppleAuthenticationOptions options)
    {
        // Use a custom CryptoProviderFactory so that keys are not cached and then disposed of, see below:
        // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1302
        var cryptoProviderFactory = new CryptoProviderFactory() { CacheSignatureProviders = false };

        options.ClientSecretGenerator ??= new DefaultAppleClientSecretGenerator(
            cache,
            timeProvider,
            cryptoProviderFactory,
            _loggerFactory.CreateLogger<DefaultAppleClientSecretGenerator>());

        options.TokenValidator ??= new DefaultAppleIdTokenValidator(
            _loggerFactory.CreateLogger<DefaultAppleIdTokenValidator>());

        if (options.ConfigurationManager is null)
        {
            if (string.IsNullOrEmpty(options.MetadataEndpoint))
            {
                throw new InvalidOperationException($"The {nameof(AppleAuthenticationOptions.MetadataEndpoint)} property must be set on the {nameof(AppleAuthenticationOptions)} instance.");
            }

            // As seen in:
            // https://github.com/dotnet/aspnetcore/blob/master/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectPostConfigureOptions.cs#L71-L102
            // need this now to successfully instantiate ConfigurationManager below.
            if (options.Backchannel is null)
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
                options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
#pragma warning restore CA2000 // Dispose objects before losing scope
                options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Apple OAuth handler");
                options.Backchannel.Timeout = options.BackchannelTimeout;
                options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
            }

            options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                options.MetadataEndpoint,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever(options.Backchannel));
        }

        options.SecurityTokenHandler ??= new JsonWebTokenHandler();
        options.TokenValidationParameters ??= new TokenValidationParameters()
        {
            CryptoProviderFactory = cryptoProviderFactory,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = options.ClientId,
            ValidIssuer = options.TokenAudience
        };
    }
}
