/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Zoom.ZoomAuthenticationConstants;

namespace AspNet.Security.OAuth.Zoom;

/// <summary>
/// Defines a set of options used by <see cref="ZoomAuthenticationHandler"/>.
/// </summary>
public class ZoomAuthenticationOptions : OAuthOptions
{
    public ZoomAuthenticationOptions()
    {
        ClaimsIssuer = ZoomAuthenticationDefaults.Issuer;
        CallbackPath = ZoomAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = ZoomAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = ZoomAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = ZoomAuthenticationDefaults.UserInformationEndpoint;
        UsePkce = true;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, ProfileFields.Id);
        ClaimActions.MapJsonKey(ClaimTypes.Name, ProfileFields.Name);
        ClaimActions.MapJsonKey(ClaimTypes.Email, ProfileFields.Email);
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, ProfileFields.GivenName);
        ClaimActions.MapJsonKey(ClaimTypes.Surname, ProfileFields.FamilyName);
        ClaimActions.MapJsonKey(Claims.Picture, ProfileFields.PictureUrl);
        ClaimActions.MapJsonKey(ClaimTypes.MobilePhone, ProfileFields.PhoneNumber);
        ClaimActions.MapJsonKey(Claims.Status, ProfileFields.Status);
        ClaimActions.MapJsonKey(Claims.Verified, ProfileFields.Verified);
        ClaimActions.MapJsonKey(Claims.PersonalMeetingUrl, ProfileFields.PersonalMeetingUrl);
    }
}
