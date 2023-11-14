/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.BattleNet;

/// <summary>
/// A class used to setup defaults for all <see cref="BattleNetAuthenticationOptions"/>.
/// </summary>
public class BattleNetPostConfigureOptions : IPostConfigureOptions<BattleNetAuthenticationOptions>
{
    /// <inheritdoc/>
    public void PostConfigure(
        string? name,
        [NotNull] BattleNetAuthenticationOptions options)
    {
        switch (options.Region)
        {
            case BattleNetAuthenticationRegion.Unified:
                options.AuthorizationEndpoint = BattleNetAuthenticationDefaults.Unified.AuthorizationEndpoint;
                options.TokenEndpoint = BattleNetAuthenticationDefaults.Unified.TokenEndpoint;
                options.UserInformationEndpoint = BattleNetAuthenticationDefaults.Unified.UserInformationEndpoint;
                break;

            case BattleNetAuthenticationRegion.America:
                options.AuthorizationEndpoint = BattleNetAuthenticationDefaults.America.AuthorizationEndpoint;
                options.TokenEndpoint = BattleNetAuthenticationDefaults.America.TokenEndpoint;
                options.UserInformationEndpoint = BattleNetAuthenticationDefaults.America.UserInformationEndpoint;
                break;

            case BattleNetAuthenticationRegion.China:
                options.AuthorizationEndpoint = BattleNetAuthenticationDefaults.China.AuthorizationEndpoint;
                options.TokenEndpoint = BattleNetAuthenticationDefaults.China.TokenEndpoint;
                options.UserInformationEndpoint = BattleNetAuthenticationDefaults.China.UserInformationEndpoint;
                break;

            case BattleNetAuthenticationRegion.Europe:
                options.AuthorizationEndpoint = BattleNetAuthenticationDefaults.Europe.AuthorizationEndpoint;
                options.TokenEndpoint = BattleNetAuthenticationDefaults.Europe.TokenEndpoint;
                options.UserInformationEndpoint = BattleNetAuthenticationDefaults.Europe.UserInformationEndpoint;
                break;

            case BattleNetAuthenticationRegion.Korea:
                options.AuthorizationEndpoint = BattleNetAuthenticationDefaults.Korea.AuthorizationEndpoint;
                options.TokenEndpoint = BattleNetAuthenticationDefaults.Korea.TokenEndpoint;
                options.UserInformationEndpoint = BattleNetAuthenticationDefaults.Korea.UserInformationEndpoint;
                break;

            case BattleNetAuthenticationRegion.Taiwan:
                options.AuthorizationEndpoint = BattleNetAuthenticationDefaults.Taiwan.AuthorizationEndpoint;
                options.TokenEndpoint = BattleNetAuthenticationDefaults.Taiwan.TokenEndpoint;
                options.UserInformationEndpoint = BattleNetAuthenticationDefaults.Taiwan.UserInformationEndpoint;
                break;

            case BattleNetAuthenticationRegion.Custom:
                break; // Do nothing

            default:
                throw new NotSupportedException($"The BattleNet region '{options.Region}' is not supported.");
        }
    }
}
