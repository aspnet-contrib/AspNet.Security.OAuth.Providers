/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Alipay;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to configure Sign in with Alipay authentication capabilities for an HTTP application pipeline.
/// </summary>
public static class AlipayAuthenticationOptionsExtensions
{
    /// <summary>
    /// Configures the application to use a specified public key to generate a client secret for the provider when using certificate signatures.
    /// </summary>
    /// <param name="options">The Alipay authentication options to configure.</param>
    /// <param name="publicKeyFile">
    /// A delegate to a method to return the <see cref="IFileInfo"/> for the public
    /// key which is passed the value of <see cref="AlipayAuthenticationOptions.ApplicationCertificateSnKeyId"/> or <see cref="AlipayAuthenticationOptions.RootCertificateSnKeyId"/>.
    /// </param>
    /// <returns>
    /// The value of the <paramref name="options"/> argument.
    /// </returns>
    public static AlipayAuthenticationOptions UsePublicKey(
        [NotNull] this AlipayAuthenticationOptions options,
        [NotNull] Func<string, IFileInfo> publicKeyFile)
    {
        options.UseCertificateSignatures = true;
        options.PublicKey = async (keyId, cancellationToken) =>
        {
            var fileInfo = publicKeyFile(keyId);

            using var stream = fileInfo.CreateReadStream();
            using var reader = new StreamReader(stream);

            return (await reader.ReadToEndAsync(cancellationToken)).AsMemory();
        };

        return options;
    }
}
