using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.WebEncoders;

namespace AspNet.Security.OAuth.LinkedIn {
    public class LinkedInAuthenticationMiddleware : OAuthAuthenticationMiddleware<LinkedInAuthenticationOptions> {
        public LinkedInAuthenticationMiddleware([NotNull] RequestDelegate next,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] IUrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> externalOptions,
            [NotNull] IOptions<LinkedInAuthenticationOptions> options,
            ConfigureOptions<LinkedInAuthenticationOptions> configureOptions = null)
            : base(next, dataProtectionProvider, loggerFactory,
                encoder, externalOptions, options, configureOptions) {
        }

        protected override AuthenticationHandler<LinkedInAuthenticationOptions> CreateHandler()
        {
            return new LinkedInAuthenticationHandler(Backchannel);
        }

    }
}