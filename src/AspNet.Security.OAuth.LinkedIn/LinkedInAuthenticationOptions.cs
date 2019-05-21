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

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "emailAddress");
            ClaimActions.MapCustomJson(ClaimTypes.Name, user => $"{GetMultiLocaleString(user, "firstName")} {GetMultiLocaleString(user, "lastName")}");
            ClaimActions.MapCustomJson(ClaimTypes.GivenName, user => GetMultiLocaleString(user, "firstName"));
            ClaimActions.MapCustomJson(ClaimTypes.Surname, user => GetMultiLocaleString(user, "lastName"));
            ClaimActions.MapCustomJson(Claims.PictureUrl, user
                => user.SelectTokens("$.profilePicture..elements[*].identifiers[0].identifier").Select(u => u.Value<string>()).LastOrDefault());
            ClaimActions.MapCustomJson(Claims.PictureUrls, user
                => string.Join(",", user.SelectTokens("$.profilePicture..elements[*].identifiers[0].identifier").Select(u => u.Value<string>())));
        }

        /// <summary>
        /// Gets or sets the email address endpoint.
        /// </summary>
        public string EmailAddressEndpoint { get; set; }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// See https://docs.microsoft.com/en-us/linkedin/consumer/integrations/self-serve/sign-in-with-linkedin for more information.
        /// </summary>
        public ISet<string> Fields { get; } = new HashSet<string>
        {
            ProfileFields.Id,
            ProfileFields.FirstName,
            ProfileFields.LastName,
            LinkedInAuthenticationConstants.EmailAddressField
        };

        /// <summary>
        /// Gets the MultiLocaleString value.
        /// First checks if a preferredLocale is returned from the payload, then try to return the value from it.
        /// Then checks if a value is available for the current UI culture.
        /// Finally, returns the first localized value.
        /// See https://docs.microsoft.com/en-us/linkedin/shared/references/v2/object-types#multilocalestring
        /// </summary>
        /// <param name="user">The payload returned by the user info endpoint.</param>
        /// <param name="propertyName">The name of the MultiLocaleString property.</param>
        /// <returns>The property value.</returns>
        private string GetMultiLocaleString(JObject user, string propertyName)
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
    }
}
