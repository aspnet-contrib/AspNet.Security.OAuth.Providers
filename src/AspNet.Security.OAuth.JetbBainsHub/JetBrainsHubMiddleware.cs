using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.JetbBainsHub {
    /// <summary>
    /// An ASP.NET Core middleware for authenticating users using JetBrains Hub OAuth 2.0.
    /// </summary>
    public class JetBrainsHubMiddleware : OAuthMiddleware<JetBrainsHubOptions> {
        /// <summary>
        /// Initializes a new <see cref="JetBrainsHubMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the HTTP pipeline to invoke.</param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="encoder"></param>
        /// <param name="sharedOptions"></param>
        /// <param name="options">Configuration options for the middleware.</param>
        public JetBrainsHubMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            IOptions<SharedAuthenticationOptions> sharedOptions,
            IOptions<JetBrainsHubOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options) {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (dataProtectionProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProtectionProvider));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            if (sharedOptions == null)
            {
                throw new ArgumentNullException(nameof(sharedOptions));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler{T}"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>
        /// An <see cref="AuthenticationHandler{T}"/> configured with the <see cref="GoogleOptions"/>
        /// supplied to the constructor.
        /// </returns>
        protected override AuthenticationHandler<JetBrainsHubOptions> CreateHandler()
        {
            return new JetBrainsHubHandler(Backchannel);
        }
    }
}