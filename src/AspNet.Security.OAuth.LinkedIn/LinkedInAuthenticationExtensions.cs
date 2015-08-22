 // ReSharper disable once CheckNamespace

using System;
using AspNet.Security.OAuth.LinkedIn;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class LinkedInAuthenticationExtensions {
        public static IServiceCollection ConfigureLinkedInAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<LinkedInAuthenticationOptions> configuration)
        {
            return services.Configure(configuration);
        }

        public static IServiceCollection ConfigureLinkedInAuthentication(
            [NotNull] this IServiceCollection services, [NotNull] string instance,
            [NotNull] Action<LinkedInAuthenticationOptions> configuration)
        {
            return services.Configure(configuration, instance);
        }

        public static IApplicationBuilder UseLinkedInAuthentication([NotNull] this IApplicationBuilder app)
        {
            return app.UseMiddleware<LinkedInAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseLinkedInAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<LinkedInAuthenticationOptions> configuration)
        {
            return app.UseMiddleware<LinkedInAuthenticationMiddleware>(
                new ConfigureOptions<LinkedInAuthenticationOptions>(configuration));
        }

        public static IApplicationBuilder UseLinkedInAuthentication(
            [NotNull] this IApplicationBuilder app, [NotNull] string instance)
        {
            return app.UseMiddleware<LinkedInAuthenticationMiddleware>(
                new ConfigureOptions<LinkedInAuthenticationOptions>(options => { }) { Name = instance });
        }

        public static IApplicationBuilder UseLinkedInAuthentication(
            [NotNull] this IApplicationBuilder app, [NotNull] string instance,
            [NotNull] Action<LinkedInAuthenticationOptions> configuration)
        {
            return app.UseMiddleware<LinkedInAuthenticationMiddleware>(
                new ConfigureOptions<LinkedInAuthenticationOptions>(configuration) { Name = instance });
        }
    }
}