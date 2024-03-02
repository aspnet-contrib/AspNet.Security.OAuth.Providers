/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Text.Json;
using Microsoft.Extensions.Options;
using static AspNet.Security.OAuth.LinkedIn.LinkedInAuthenticationConstants;

namespace AspNet.Security.OAuth.LinkedIn;

/// <summary>
/// Defines a set of options used by <see cref="LinkedInAuthenticationHandler"/>.
/// </summary>
public class LinkedInAuthenticationOptions : OAuthOptions
{
    public LinkedInAuthenticationOptions()
    {
        ClaimsIssuer = LinkedInAuthenticationDefaults.Issuer;
        CallbackPath = LinkedInAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = LinkedInAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = LinkedInAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = LinkedInAuthenticationDefaults.UserInformationEndpoint;
        EmailAddressEndpoint = LinkedInAuthenticationDefaults.EmailAddressEndpoint;

        Scope.Add("openid");
        Scope.Add("profile");
        Scope.Add("email");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, ProfileFields.Id);
        ClaimActions.MapJsonKey(ClaimTypes.Name, ProfileFields.Name);
        ClaimActions.MapJsonKey(ClaimTypes.Email, ProfileFields.Email);
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, ProfileFields.GivenName);
        ClaimActions.MapJsonKey(ClaimTypes.Surname, ProfileFields.FamilyName);
        ClaimActions.MapJsonKey(Claims.Picture, Claims.Picture);
        ClaimActions.MapJsonKey(Claims.EmailVerified, Claims.EmailVerified);
    }

    /// <summary>
    /// Gets or sets the email address endpoint.
    /// </summary>
    public string EmailAddressEndpoint { get; set; }
}
