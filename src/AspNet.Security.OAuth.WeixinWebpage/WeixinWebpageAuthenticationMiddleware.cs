/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Text.Encodings.Web;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

namespace AspNet.Security.OAuth.WeixinWebpage
{
    public class WeixinWebpageAuthenticationMiddleware : OAuthMiddleware<WeixinWebpageAuthenticationOptions>
    {
        public WeixinWebpageAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] IMemoryCache cache,
            [NotNull] UrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> sharedOptions,
            [NotNull] IOptions<WeixinWebpageAuthenticationOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options)
        {
            options.Value.StateDataFormat = new StoreInCacheFormat(cache, options.Value.RemoteAuthenticationTimeout);
        }

        protected override AuthenticationHandler<WeixinWebpageAuthenticationOptions> CreateHandler()
        {
            return new WeixinWebpageAuthenticationHandler(Backchannel);
        }
    }
}
