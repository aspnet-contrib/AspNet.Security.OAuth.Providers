/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using static AspNet.Security.OAuth.Csdn.CsdnAuthenticationConstants;

namespace AspNet.Security.OAuth.Csdn
{
    /// <summary>
    /// Defines a set of options used by <see cref="CsdnAuthenticationHandler"/>.
    /// </summary>
    public class CsdnAuthenticationOptions : OAuthOptions
    {
        public CsdnAuthenticationOptions()
        {
            ClaimsIssuer = CsdnAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(CsdnAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = CsdnAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = CsdnAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = CsdnAuthenticationDefaults.UserInformationEndpoint;

            //Scope.Add("snsapi_login");
            //Scope.Add("snsapi_userinfo");

            //Scope.Add("basic");

            //ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            //"1"表示男，"0"表示女。
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex", ClaimValueTypes.Integer);

            //ClaimActions.MapJsonKey(Claims.UserId, "userid");
            ClaimActions.MapJsonKey(Claims.UserName, "username");
            ClaimActions.MapJsonKey(Claims.Job, "job");
            ClaimActions.MapJsonKey(Claims.WorkYear, "portrait");
            ClaimActions.MapJsonKey(Claims.Website, "userdetail");
            ClaimActions.MapJsonKey(Claims.Description, "birthday");
            //ClaimActions.MapJsonKey(Claims.Marriage, "marriage");
            //ClaimActions.MapJsonKey(Claims.Blood, "blood");
            //ClaimActions.MapJsonKey(Claims.Figure, "figure");
            //ClaimActions.MapJsonKey(Claims.Constellation, "constellation");
            //ClaimActions.MapJsonKey(Claims.Education, "education");
            //ClaimActions.MapJsonKey(Claims.Trade, "trade");
            //ClaimActions.MapJsonKey(Claims.Job, "job");
        }
    }
}
