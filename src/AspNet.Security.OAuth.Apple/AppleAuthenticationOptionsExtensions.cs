/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IO;
using AspNet.Security.OAuth.Apple;
using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to configure Sign in with Apple authentication capabilities for an HTTP application pipeline.
    /// </summary>
    public static class AppleAuthenticationOptionsExtensions
    {
        /// <summary>
        /// Configures the application to use a specified private to generate a client secret for the provider.
        /// </summary>
        /// <param name="options">The Apple authentication options to configure.</param>
        /// <param name="privateKeyFile">
        /// A delegate to a method to return the <see cref="IFileInfo"/> for the private
        /// key which is passed the value of <see cref="AppleAuthenticationOptions.KeyId"/>.
        /// </param>
        /// <returns>
        /// The value of the <paramref name="options"/> argument.
        /// </returns>
        public static AppleAuthenticationOptions UsePrivateKey(
            [NotNull] this AppleAuthenticationOptions options,
            [NotNull] Func<string, IFileInfo> privateKeyFile)
        {
            options.GenerateClientSecret = true;
            options.PrivateKeyBytes = async (keyId, _) =>
            {
                var fileInfo = privateKeyFile(keyId);

                using var stream = fileInfo.CreateReadStream();
                using var reader = new StreamReader(stream);

                string privateKey = await reader.ReadToEndAsync();

                if (privateKey.StartsWith("-----BEGIN PRIVATE KEY-----", StringComparison.Ordinal))
                {
                    string[] lines = privateKey.Split('\n');
                    privateKey = string.Join(string.Empty, lines[1..^1]);
                }

                return Convert.FromBase64String(privateKey);
            };

            return options;
        }
    }
}
