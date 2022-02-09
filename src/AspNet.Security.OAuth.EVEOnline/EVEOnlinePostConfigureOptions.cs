/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AspNet.Security.OAuth.EVEOnline;

/// <summary>
/// Used to setup defaults for all <see cref="EVEOnlineAuthenticationOptions"/>.
/// </summary>
public class EVEOnlinePostConfigureOptions : IPostConfigureOptions<EVEOnlineAuthenticationOptions>
{
    /// <inheritdoc />
    public void PostConfigure(
        [NotNull] string name,
        [NotNull] EVEOnlineAuthenticationOptions options)
    {
        if (options.SecurityTokenHandler == null)
        {
            options.SecurityTokenHandler = new JsonWebTokenHandler();
        }
    }
}
