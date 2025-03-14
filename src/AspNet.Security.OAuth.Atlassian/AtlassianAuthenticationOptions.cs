/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Linq;
using System.Security.Claims;
using static AspNet.Security.OAuth.Atlassian.AtlassianOAuthenticationConstants;

namespace AspNet.Security.OAuth.Atlassian;

public partial class AtlassianAuthenticationOptions : OAuthOptions
{
    public AtlassianAuthenticationOptions()
    {
        ClaimsIssuer = AtlassianAuthenticationDefaults.Issuer;
        CallbackPath = AtlassianAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = AtlassianAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = AtlassianAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = AtlassianAuthenticationDefaults.UserInformationEndpoint;

        AdditionalAuthorizationParameters.Add("audience", "api.atlassian.com");
        AdditionalAuthorizationParameters.Add("prompt", "consent");

        Scope.Add("read:me");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "account_id");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");

        ClaimActions.MapJsonKey(Claims.AccountType, "account_type");
        ClaimActions.MapJsonKey(Claims.Picture, "picture");
        ClaimActions.MapJsonKey(Claims.AccountStatus, "account_status");
        ClaimActions.MapJsonKey(Claims.Nickname, "nickname");
        ClaimActions.MapJsonKey(Claims.ZoneInfo, "zoneinfo");
        ClaimActions.MapJsonKey(Claims.Locale, "locale");

        ClaimActions.MapJsonSubKey(Claims.JobTitle, "extended_profile", "job_title");
        ClaimActions.MapJsonSubKey(Claims.Organization, "extended_profile", "organization");
        ClaimActions.MapJsonSubKey(Claims.Department, "extended_profile", "department");
        ClaimActions.MapJsonSubKey(Claims.Location, "extended_profile", "location");
    }
}
