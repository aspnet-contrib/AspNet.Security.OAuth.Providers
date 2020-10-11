/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Apple.Internal
{
    internal sealed class CustomCryptoProviderFactory : CryptoProviderFactory
    {
        public CustomCryptoProviderFactory()
            : base()
        {
            CacheSignatureProviders = false;
        }
    }
}
