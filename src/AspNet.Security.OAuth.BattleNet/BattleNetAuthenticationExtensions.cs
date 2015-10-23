﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.BattleNet;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Builder {
    public static class BattleNetAuthenticationExtensions {
        public static IApplicationBuilder UseBattleNetAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] BattleNetAuthenticationOptions options) {
            return app.UseMiddleware<BattleNetAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseBattleNetAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<BattleNetAuthenticationOptions> configuration) {
            var options = new BattleNetAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<BattleNetAuthenticationMiddleware>(options);
        }
    }
}
