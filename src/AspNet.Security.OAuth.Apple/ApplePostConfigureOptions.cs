/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.IdentityModel.Tokens.Jwt;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// A class used to setup defaults for all <see cref="AppleAuthenticationOptions"/>.
    /// </summary>
    public class ApplePostConfigureOptions : IPostConfigureOptions<AppleAuthenticationOptions>
    {
        /// <inheritdoc/>
        public void PostConfigure(
            [NotNull] string name,
            [NotNull] AppleAuthenticationOptions options)
        {
            if (options.JwtSecurityTokenHandler is null)
            {
                options.JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            }
        }
    }
}
