using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using JetBrains.Annotations;

namespace AspNet.Security.OAuth.Weibo
{
    /// <summary>
    /// An ASP.NET Core middleware for authenticating users using Weibo OAuth 2.0.
    /// </summary>
    public class WeiboAuthenticationMiddleware : OAuthMiddleware<WeiboAuthenticationOptions>
    {
        public WeiboAuthenticationMiddleware(
            [NotNull] RequestDelegate next,
            [NotNull] IDataProtectionProvider dataProtectionProvider,
            [NotNull] ILoggerFactory loggerFactory,
            [NotNull] UrlEncoder encoder,
            [NotNull] IOptions<SharedAuthenticationOptions> sharedOptions,
            [NotNull] IOptions<WeiboAuthenticationOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options)
        {
        }

        protected override AuthenticationHandler<WeiboAuthenticationOptions> CreateHandler()
        {
            return new WeiboAuthenticationHandler(Backchannel);
        }
    }
}
