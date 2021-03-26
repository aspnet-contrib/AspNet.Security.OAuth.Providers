/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Moodle.MoodleAuthenticationConstants;

namespace AspNet.Security.OAuth.Moodle
{
    /// <summary>
    /// Defines a set of options used by <see cref="MoodleAuthenticationHandler"/>.
    /// </summary>
    public class MoodleAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoodleAuthenticationOptions"/> class.
        /// </summary>
        public MoodleAuthenticationOptions()
        {
            AuthorizationEndpoint = MoodleAuthenticationDefaults.AuthorizationEndpoint;
            CallbackPath = MoodleAuthenticationDefaults.CallbackPath;
            TokenEndpoint = MoodleAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = MoodleAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("user_info");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "username");

            ClaimActions.MapCustomJson(ClaimTypes.Name,
                e =>
                    e.GetString("lang")?.StartsWith("zh", System.StringComparison.InvariantCultureIgnoreCase) ?? false ?
                        $"{e.GetString("lastname")}{e.GetString("firstname")}" :
                    $"{e.GetString("firstname")} {e.GetString("lastname")}");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.MobilePhone, "phone1");
            ClaimActions.MapJsonKey(Claims.FirstName, "firstname");
            ClaimActions.MapJsonKey(Claims.LastName, "lastname");
            ClaimActions.MapJsonKey(Claims.IdNumber, "idnumber");
            ClaimActions.MapJsonKey(Claims.MoodleId, "id");
            ClaimActions.MapJsonKey(Claims.Language, "lang");
        }

        /// <summary>
        /// Use the moodle url to initialize <see cref="OAuthOptions.AuthorizationEndpoint" />,
        /// <see cref="OAuthOptions.TokenEndpoint" /> and <see cref="OAuthOptions.UserInformationEndpoint" />.
        /// </summary>
        /// <param name="moodleSite">Url for your moodle site, like 'http://moodledomain.com'</param>
        /// <returns>Options changed.</returns>
        public MoodleAuthenticationOptions UseMoodleSite(string moodleSite)
        {
            AuthorizationEndpoint =
                    $"{moodleSite}/local/oauth/login.php";
            TokenEndpoint = $"{moodleSite}/local/oauth/token.php";
            UserInformationEndpoint = $"{moodleSite}/local/oauth/user_info.php";
            return this;
        }
    }
}
