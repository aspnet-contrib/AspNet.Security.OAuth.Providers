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

namespace AspNet.Security.OAuth.VisualStudio
{
    public class VisualStudioAuthenticationMiddleware : OAuthMiddleware<VisualStudioAuthenticationOptions>
    {
        public VisualStudioAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] UrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> externalOptions,
            [NotNull] IOptions<VisualStudioAuthenticationOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, externalOptions, options)
        {
        }

        protected override AuthenticationHandler<VisualStudioAuthenticationOptions> CreateHandler()
        {
            return new VisualStudioAuthenticationHandler(Backchannel);
        }
    }
}