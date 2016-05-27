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
using System;

namespace AspNet.Security.OAuth.LinkedIn {
    public class LinkedInAuthenticationMiddleware : OAuthMiddleware<LinkedInAuthenticationOptions> {
        public LinkedInAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] IOptions<LinkedInAuthenticationOptions> options,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] UrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> externalOptions)
            : base(next, dataProtectionProvider, loggerFactory, encoder, externalOptions, options) {
            if (!options.Value.UserInformationEndpoint.Contains("~") && options.Value.Fields.Count > 0)
            {
                var message = "The UserInformationEndpoint is improperly formatted. The endpoint must contain a '~' to append the supplied fields.";
                Logger.LogError(message);
                throw new ArgumentException(message);
            }
        }

        protected override AuthenticationHandler<LinkedInAuthenticationOptions> CreateHandler() {
            return new LinkedInAuthenticationHandler(Backchannel);
        }
    }
}