/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.SuperOffice.Implementation
{
    internal sealed class DefaultSuperOfficeConfigurationManager : SuperOfficeAuthenticationConfigurationManager
    {
        private readonly ISystemClock _clock;
        private readonly ILogger _logger;

        private string _jwks = string.Empty;
        private DateTimeOffset _reloadJwksAfter;

        public DefaultSuperOfficeConfigurationManager(
            [NotNull] ISystemClock clock,
            [NotNull] ILogger<DefaultSuperOfficeConfigurationManager> logger)
        {
            _clock = clock;
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task<JsonWebKeySet> GetPublicKeySetAsync([NotNull] SuperOfficeValidateIdTokenContext context)
        {
            var utcNow = _clock.UtcNow;

            if (string.IsNullOrWhiteSpace(_jwks) || _reloadJwksAfter < utcNow)
            {
                _logger.LogInformation($"Loading SuperOffice JwksUri from {context.Options.JwksEndpoint}.");

                _jwks = await LoadJwksAsync(context, context.Options.JwksEndpoint);
                _reloadJwksAfter = utcNow.Add(context.Options.JwksCacheLifetime);

                _logger.LogInformation(
                    $"Loaded SuperOffice JwksUrl from {context.Options.JwksEndpoint} and obtained public keys. Keys will be reloaded at or after {_reloadJwksAfter}.");
            }

            return JsonWebKeySet.Create(_jwks);
        }

        private async Task<string> LoadJwksAsync(
            [NotNull] SuperOfficeValidateIdTokenContext context,
            [NotNull] string jwksUrl)
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
    }
}
