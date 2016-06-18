/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JetBrains.Annotations;

namespace Microsoft.AspNetCore.Authentication.Yammer {
    /// <summary>
    /// An ASP.NET Core middleware for authenticating users using Yammer.
    /// </summary>
    public class YammerAuthenticationMiddleware : OAuthMiddleware<YammerAuthenticationOptions> {
        /// <summary>
        /// Initializes a new <see cref="YammerAuthenticationMiddleware"/>.
        /// </summary>
        public YammerAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] UrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> sharedOptions,
            [NotNull] IOptions<YammerAuthenticationOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options) {
        }

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler{T}"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler{T}"/> configured with the <see cref="YammerAuthenticationOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<YammerAuthenticationOptions> CreateHandler() {
            return new YammerAuthenticationHandler(Backchannel);
        }
    }
}