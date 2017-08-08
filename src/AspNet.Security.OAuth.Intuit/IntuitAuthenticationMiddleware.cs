/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Text.Encodings.Web;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Intuit
{
    public class IntuitAuthenticationMiddleware : OAuthMiddleware<IntuitAuthenticationOptions>
    {
        public IntuitAuthenticationMiddleware(
             RequestDelegate next,
             IDataProtectionProvider dataProtectionProvider,
             ILoggerFactory loggerFactory,
             UrlEncoder encoder,
             IOptions<SharedAuthenticationOptions> sharedOptions,
             IOptions<IntuitAuthenticationOptions> options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options)
        {
        }

        protected override AuthenticationHandler<IntuitAuthenticationOptions> CreateHandler()
        {


            var obj = new IntuitAuthenticationHandler(Backchannel);
            return obj;
        }
    }
}