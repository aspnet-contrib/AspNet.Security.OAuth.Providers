/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JetBrains.Annotations;

namespace AspNet.Security.OAuth.VisualStudio {
    /// <summary>
    /// An ASP.NET Core middleware for authenticating users using VisualStudio.
    /// </summary>
    public class VisualStudioAuthenticationMiddleware : OAuthMiddleware<VisualStudioAuthenticationOptions> {
        /// <summary>
        /// Initializes a new <see cref="VisualStudioAuthenticationMiddleware"/>.
        /// </summary>
        public VisualStudioAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] IOptions<VisualStudioAuthenticationOptions> options,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] UrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> externalOptions)
            : base(next, dataProtectionProvider, loggerFactory, encoder, externalOptions, options) {
        }

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler{T}"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler{T}"/> configured with the <see cref="VisualStudioAuthenticationOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<VisualStudioAuthenticationOptions> CreateHandler() {
            return new VisualStudioAuthenticationHandler(Backchannel);
        }
    }
}