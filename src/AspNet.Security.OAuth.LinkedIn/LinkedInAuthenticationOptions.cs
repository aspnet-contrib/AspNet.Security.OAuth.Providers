/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using static AspNet.Security.OAuth.LinkedIn.LinkedInAuthenticationConstants;

namespace AspNet.Security.OAuth.LinkedIn
{
    /// <summary>
    /// Defines a set of options used by <see cref="LinkedInAuthenticationHandler"/>.
    /// </summary>
    public class LinkedInAuthenticationOptions : OAuthOptions
    {
        public LinkedInAuthenticationOptions()
        {
            ClaimsIssuer = LinkedInAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(LinkedInAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = LinkedInAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = LinkedInAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = LinkedInAuthenticationDefaults.UserInformationEndpoint;
            EmailAddressEndpoint = LinkedInAuthenticationDefaults.EmailAddressEndpoint;

            Scope.Add("r_liteprofile");
            Scope.Add("r_emailaddress");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, ProfileFields.Id);
            ClaimActions.MapJsonKey(ClaimTypes.Email, LinkedInAuthenticationConstants.EmailAddressField);
            ClaimActions.MapCustomJson(ClaimTypes.Name, user => GetFullName(user));
            ClaimActions.MapCustomJson(ClaimTypes.GivenName, user => GetMultiLocaleString(user, ProfileFields.FirstName));
            ClaimActions.MapCustomJson(ClaimTypes.Surname, user => GetMultiLocaleString(user, ProfileFields.LastName));
            ClaimActions.MapCustomJson(Claims.PictureUrl, user => GetPictureUrls(user)?.LastOrDefault());
            ClaimActions.MapCustomJson(Claims.PictureUrls, user => string.Join(",", GetPictureUrls(user)));
        }

        /// <summary>
        /// Gets or sets the email address endpoint.
        /// </summary>
        public string EmailAddressEndpoint { get; set; }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// See <a>https://docs.microsoft.com/en-us/linkedin/consumer/integrations/self-serve/sign-in-with-linkedin</a> for more information.
        /// </summary>
        public ISet<string> Fields { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ProfileFields.Id,
            ProfileFields.FirstName,
            ProfileFields.LastName,
            LinkedInAuthenticationConstants.EmailAddressField
        };

        /// <summary>
        /// Gets the <c>MultiLocaleString</c> value.
        /// First checks if a preferredLocale is returned from the payload, then try to return the value from it.
        /// Then checks if a value is available for the current UI culture.
        /// Finally, returns the first localized value.
        /// See <a>https://docs.microsoft.com/en-us/linkedin/shared/references/v2/object-types#multilocalestring</a>
        /// </summary>
        /// <param name="user">The payload returned by the user info endpoint.</param>
        /// <param name="propertyName">The name of the <c>MultiLocaleString</c> property.</param>
        /// <returns>The property value.</returns>
        private static string GetMultiLocaleString(JObject user, string propertyName)
        {
            if (user[propertyName] == null)
            {
                return null;
            }

            var preferredLocale = user[propertyName]["preferredLocale"];
            const string localizedKey = "localized";

            if (preferredLocale != null)
            {
                var preferredKey = $"{preferredLocale["language"]}_{preferredLocale["country"]}";
                var preferredLocalizedValue = user[propertyName][localizedKey][preferredKey];
                if (preferredLocalizedValue != null)
                {
                    return preferredLocalizedValue.Value<string>();
                }
            }

            var currentUiKey = Thread.CurrentThread.CurrentUICulture.ToString().Replace('-', '_');
            var currentUiLocalizedValue = user[propertyName][localizedKey][currentUiKey];
            if (currentUiLocalizedValue != null)
            {
                return currentUiLocalizedValue.Value<string>();
            }

            return user[propertyName][localizedKey].First.Value<string>();
        }

        private static string GetFullName(JObject user)
        {
            var nameParts = new string[]
            {
                GetMultiLocaleString(user, ProfileFields.FirstName),
                GetMultiLocaleString(user, ProfileFields.LastName)
            };

            return string.Join(" ", nameParts.Where(s => !string.IsNullOrWhiteSpace(s)));
        }

        private static IEnumerable<string> GetPictureUrls(JObject user)
        {
            var profilePictureElements = user.Value<JObject>("profilePicture")
                ?.Value<JObject>("displayImage~")
                ?.Value<JArray>("elements");

            if (profilePictureElements == null)
            {
                return null;
            }

            return (from address in profilePictureElements
                    where address.Value<string>("authorizationMethod") == "PUBLIC"
                    select address.Value<JArray>("identifiers")?.First()?.Value<string>("identifier"));
        }
    }
}
