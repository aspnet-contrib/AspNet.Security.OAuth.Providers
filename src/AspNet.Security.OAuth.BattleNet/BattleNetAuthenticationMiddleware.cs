/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.Framework.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.WebEncoders;

namespace AspNet.Security.OAuth.BattleNet {
    public class BattleNetAuthenticationMiddleware : OAuthMiddleware<BattleNetAuthenticationOptions> {
        public BattleNetAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] IUrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> externalOptions,
            BattleNetAuthenticationOptions options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, externalOptions, options) {
        }

        protected override AuthenticationHandler<BattleNetAuthenticationOptions> CreateHandler() {
            return new BattleNetAuthenticationHandler(Backchannel);
        }
    }
}
