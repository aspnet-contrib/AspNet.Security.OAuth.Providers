/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.WordPress;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class WordPressAuthenticationExtensions {
        public static IApplicationBuilder UseWordPressAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] WordPressAuthenticationOptions options) {
            return app.UseMiddleware<WordPressAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseWordPressAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<WordPressAuthenticationOptions> configuration) {
            var options = new WordPressAuthenticationOptions();
            configuration(options);

            return app.UseWordPressAuthentication(options);
        }
    }
}