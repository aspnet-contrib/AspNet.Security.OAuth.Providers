/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.WordPress;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNet.Builder {
    public static class WordPressAuthenticationExtensions {
        public static IServiceCollection ConfigureWordPressAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<WordPressAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IServiceCollection ConfigureWordPressAuthentication(
            [NotNull] this IServiceCollection services, [NotNull] string instance,
            [NotNull] Action<WordPressAuthenticationOptions> configuration) {
            return services.Configure(configuration, instance);
        }

        public static IApplicationBuilder UseWordPressAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<WordPressAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseWordPressAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<WordPressAuthenticationOptions> configuration) {
            return app.UseMiddleware<WordPressAuthenticationMiddleware>(
                new ConfigureOptions<WordPressAuthenticationOptions>(configuration));
        }

        public static IApplicationBuilder UseWordPressAuthentication(
            [NotNull] this IApplicationBuilder app, [NotNull] string instance) {
            return app.UseMiddleware<WordPressAuthenticationMiddleware>(
                new ConfigureOptions<WordPressAuthenticationOptions>(options => { }) { Name = instance });
        }

        public static IApplicationBuilder UseWordPressAuthentication(
            [NotNull] this IApplicationBuilder app, [NotNull] string instance,
            [NotNull] Action<WordPressAuthenticationOptions> configuration) {
            return app.UseMiddleware<WordPressAuthenticationMiddleware>(
                new ConfigureOptions<WordPressAuthenticationOptions>(configuration) { Name = instance });
        }
    }
}