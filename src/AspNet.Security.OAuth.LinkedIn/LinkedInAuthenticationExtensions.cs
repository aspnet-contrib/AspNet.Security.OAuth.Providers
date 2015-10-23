/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.LinkedIn;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class LinkedInAuthenticationExtensions {
        public static IApplicationBuilder UseLinkedInAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] LinkedInAuthenticationOptions options) {
            return app.UseMiddleware<LinkedInAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseLinkedInAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<LinkedInAuthenticationOptions> configuration) {
            var options = new LinkedInAuthenticationOptions();
            configuration(options);

            return app.UseLinkedInAuthentication(options);
        }
    }
}