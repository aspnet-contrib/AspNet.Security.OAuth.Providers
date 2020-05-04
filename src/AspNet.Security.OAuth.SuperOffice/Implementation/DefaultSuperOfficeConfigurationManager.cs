/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.SuperOffice.Implementation
{
    internal sealed class DefaultSuperOfficeConfigurationManager : SuperOfficeAuthenticationConfigurationManager
    {
        private readonly ISystemClock _clock;
        private readonly ILogger _logger;

        private string _publicKey = string.Empty;
        private DateTimeOffset _reloadKeysAfter;

        public DefaultSuperOfficeConfigurationManager(
            ISystemClock clock,
            ILogger<DefaultSuperOfficeConfigurationManager> logger)
        {
            _clock = clock;
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task<JsonWebKeySet> GetPublicKeySetAsync(SuperOfficeValidateIdTokenContext context)
        {
            if (string.IsNullOrEmpty(Configuration.JwksUri))
            {
                await LoadConfigurationAsync(context);
            }

            var utcNow = _clock.UtcNow;

            if (string.IsNullOrWhiteSpace(_publicKey) || _reloadKeysAfter < utcNow)
            {
                _logger.LogInformation($"Loading SuperOffice JwksUri from {Configuration.JwksUri}.");

                _publicKey = await LoadJwksAsync(context, Configuration.JwksUri);
                _reloadKeysAfter = utcNow.Add(context.Options.PublicKeyCacheLifetime);

                _logger.LogInformation(
                    $"Loaded SuperOffice JwksUrl from {Configuration.JwksUri} and obtained public keys. Keys will be reloaded at or after {_reloadKeysAfter}.");
            }

            return JsonWebKeySet.Create(_publicKey);
        }

        /// <inheritdoc />
        public override async Task LoadConfigurationAsync(SuperOfficeValidateIdTokenContext context)
        {
            using var response = await context.Options.Backchannel.GetAsync(context.Options.ConfigurationEndpoint, context.HttpContext.RequestAborted);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("An error occurred while retrieving the configuration from SuperOffice: the remote server " +
                                 "returned a {Status} response with the following payload: {Headers} {Body}.",
                                 /* Status: */ response.StatusCode,
                                 /* Headers: */ response.Headers.ToString(),
                                 /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the configuration from SuperOffice.");
            }

            byte[] jsonBytes = await response.Content.ReadAsByteArrayAsync();
            Configuration = GetConfiguration(jsonBytes);
        }

        private async Task<string> LoadJwksAsync(SuperOfficeValidateIdTokenContext context, string jwksUrl)
        {
            using var response = await context.Options.Backchannel.GetAsync(jwksUrl, context.HttpContext.RequestAborted);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("An error occurred while retrieving the keys from SuperOffice: the remote server " +
                                 "returned a {Status} response with the following payload: {Headers} {Body}.",
                                 /* Status: */ response.StatusCode,
                                 /* Headers: */ response.Headers.ToString(),
                                 /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the keys from SuperOffice.");
            }

            return await response.Content.ReadAsStringAsync();
        }

        private static SuperOfficeAuthenticationConfiguration GetConfiguration(byte[] jsonAsBytes)
        {
            var jsonSpan = new ReadOnlySpan<byte>(jsonAsBytes);
            return JsonSerializer.Deserialize<SuperOfficeAuthenticationConfiguration>(jsonSpan);
        }
    }
}
