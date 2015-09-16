﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Onshape;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class OnshapeAuthenticationExtensions {
        public static IServiceCollection ConfigureGitHubAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<OnshapeAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseGitHubAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<GitHubAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseGitHubAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<OnshapeAuthenticationOptions> configuration) {
            return app.UseMiddleware<GitHubAuthenticationMiddleware>(
                new ConfigureOptions<OnshapeAuthenticationOptions>(configuration));
        }
    }
}
