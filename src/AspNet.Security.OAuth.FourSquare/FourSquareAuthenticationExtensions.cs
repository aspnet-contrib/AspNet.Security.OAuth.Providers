/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.FourSquare;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class FourSquareAuthenticationExtensions {
        public static IApplicationBuilder UseFoursquareAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] FourSquareAuthenticationOptions options) {
            return app.UseMiddleware<FourSquareAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseFoursquareAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<FourSquareAuthenticationOptions> configuration)
        {
            var options = new FourSquareAuthenticationOptions();
            configuration(options);

            return app.UseFoursquareAuthentication(options);
        }
    }      
    
}
