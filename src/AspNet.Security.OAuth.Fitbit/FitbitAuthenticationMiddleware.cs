﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.WebEncoders;

namespace AspNet.Security.OAuth.Fitbit {
    public class FitbitAuthenticationMiddleware : OAuthAuthenticationMiddleware<FitbitAuthenticationOptions> {
        public FitbitAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] IUrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> externalOptions,
            [NotNull] IOptions<FitbitAuthenticationOptions> options,
            ConfigureOptions<FitbitAuthenticationOptions> configureOptions = null)
            : base(next, dataProtectionProvider, loggerFactory,
                   encoder, externalOptions, options, configureOptions) {
        }

        protected override AuthenticationHandler<FitbitAuthenticationOptions> CreateHandler() {
            return new FitbitAuthenticationHandler(Backchannel);
        }
    }
}
