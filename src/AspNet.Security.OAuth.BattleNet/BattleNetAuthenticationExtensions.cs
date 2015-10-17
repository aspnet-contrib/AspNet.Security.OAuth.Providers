/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.BattleNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Extensions.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class BattleNetAuthenticationExtensions {
        public static IServiceCollection ConfigureBattleNetAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<BattleNetAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseBattleNetAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<BattleNetAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseBattleNetAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<BattleNetAuthenticationOptions> configuration) {
            return app.UseMiddleware<BattleNetAuthenticationMiddleware>(new ConfigureOptions<BattleNetAuthenticationOptions>(configuration));
        }
    }
}
