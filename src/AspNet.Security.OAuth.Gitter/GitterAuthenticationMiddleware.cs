/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace AspNet.Security.OAuth.Gitter
{
    public class GitterAuthenticationMiddleware : OAuthMiddleware<GitterAuthenticationOptions>
    {
        public GitterAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] UrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> sharedOptions,
            [NotNull] IOptions<GitterAuthenticationOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options)
        {
        }

        protected override AuthenticationHandler<GitterAuthenticationOptions> CreateHandler()
        {
            return new GitterAuthenticationHandler(Backchannel);
        }
    }
}