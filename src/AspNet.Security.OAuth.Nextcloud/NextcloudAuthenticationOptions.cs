using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using static AspNet.Security.OAuth.Nextcloud.NextcloudAuthenticationConstants;

namespace AspNet.Security.OAuth.Nextcloud
{
    public class NextcloudAuthenticationOptions : OAuthOptions
    {
        public NextcloudAuthenticationOptions()
        {
            ClaimsIssuer = NextcloudAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(NextcloudAuthenticationDefaults.CallbackPath);

            ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user =>
            {
                return user["ocs"]?["data"]?.Value<string>("id");
            });

            ClaimActions.MapCustomJson(ClaimTypes.Name, user =>
            {
                return user["ocs"]?["data"]?.Value<string>("id");
            });

            ClaimActions.MapCustomJson(Claims.DisplayName, user =>
            {
                return user["ocs"]?["data"]?.Value<string>("displayname");
            });

            ClaimActions.MapCustomJson(ClaimTypes.Email, user =>
            {
                return user["ocs"]?["data"]?.Value<string>("email");
            });

            ClaimActions.MapCustomJson(Claims.Groups, user =>
            {
                var groups = (JArray)user["ocs"]?["data"]?["groups"];
                return string.Join(",", groups.ToList());
            });

            ClaimActions.MapCustomJson(Claims.Enabled, user =>
            {
                return user["ocs"]?["data"]?.Value<string>("enabled");
            });

            ClaimActions.MapCustomJson(Claims.Language, user =>
            {
                return user["ocs"]?["data"]?.Value<string>("language");
            });

            ClaimActions.MapCustomJson(Claims.Locale, user =>
            {
                return user["ocs"]?["data"]?.Value<string>("locale");
            });
        }
    }
}
