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

namespace AspNet.Security.OAuth.Amazon
{
    /// <summary>
    /// An ASP.NET Core middleware for authenticating users using Amazon.
    /// </summary>
    public class AmazonAuthenticationMiddleware : OAuthMiddleware<AmazonAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the HTTP pipeline to invoke.</param>
        /// <param name="dataProtectionProvider">The <see cref="IDataProtectionProvider"/> to use.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="encoder">The <see cref="UrlEncoder"/> to use.</param>
        /// <param name="sharedOptions">The <see cref="SharedAuthenticationOptions"/> configuration options for this middleware.</param>
        /// <param name="options">Configuration options for the middleware.</param>
        public AmazonAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] UrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> sharedOptions,
            [NotNull] IOptions<AmazonAuthenticationOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options)
        {
        }

        /// <inheritdoc />
        protected override AuthenticationHandler<AmazonAuthenticationOptions> CreateHandler()
        {
            return new AmazonAuthenticationHandler(Backchannel);
        }
    }
}
