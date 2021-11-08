/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.ServiceChannel.ServiceChannelAuthenticationConstants;

namespace AspNet.Security.OAuth.ServiceChannel;

/// <summary>
/// Defines a set of options used by <see cref="ServiceChannelAuthenticationHandler"/>.
/// </summary>
public class ServiceChannelAuthenticationOptions : OAuthOptions
{
    public ServiceChannelAuthenticationOptions()
    {
        ClaimsIssuer = ServiceChannelAuthenticationDefaults.Issuer;
        CallbackPath = ServiceChannelAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = ServiceChannelAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = ServiceChannelAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = ServiceChannelAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "UserProfile", "UserId");
        ClaimActions.MapJsonSubKey(ClaimTypes.Name, "UserProfile", "UserName");
        ClaimActions.MapJsonSubKey(ClaimTypes.Email, "UserProfile", "Email");
        ClaimActions.MapJsonSubKey(Claims.ProviderId, "UserProfile", "ProviderId");
        ClaimActions.MapJsonSubKey(Claims.ProviderName, "UserProfile", "ProviderName");
    }
}
