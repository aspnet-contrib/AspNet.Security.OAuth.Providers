/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Text.Encodings.Web;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.DataProtection;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;

namespace AspNet.Security.OAuth.Yahoo {
    public class YahooAuthenticationMiddleware : OAuthMiddleware<YahooAuthenticationOptions> {
        public YahooAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] YahooAuthenticationOptions options,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] UrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> sharedOptions)
            : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options) {
        }

        protected override AuthenticationHandler<YahooAuthenticationOptions> CreateHandler() {
            return new YahooAuthenticationHandler(Backchannel);
        }
    }
}