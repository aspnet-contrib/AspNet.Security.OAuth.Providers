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
            ClaimActions.MapCustomJson(Claims.PictureUrls, user =>
            {
                var urls = GetPictureUrls(user);
                return urls == null ? null : string.Join(",", urls);
            });
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
        /// Gets or sets a <c>MultiLocaleString</c> resolver, a function which takes all localized values 
        /// and an eventual preferred locale from the member and returns the selected localized value.
        /// The default implementation resolve it in this order:
        /// 1. Returns the <c>preferredLocale</c> value if it is set and has a value.
        /// 2. Returns the value corresponding to the <see cref="Thread.CurrentUICulture"/> if it exists.
        /// 3. Returns the first value.
        /// </summary>
        /// <see cref="DefaultMultiLocaleStringResolver(IReadOnlyDictionary{string, string}, string)"/>
        public Func<IReadOnlyDictionary<string, string>, string, string> MultiLocaleStringResolver { get; set; } = DefaultMultiLocaleStringResolver;

        /// <summary>
        /// Gets the <c>MultiLocaleString</c> value using the configured resolver.
        /// See <a>https://docs.microsoft.com/en-us/linkedin/shared/references/v2/object-types#multilocalestring</a>
        /// </summary>
        /// <param name="user">The payload returned by the user info endpoint.</param>
        /// <param name="propertyName">The name of the <c>MultiLocaleString</c> property.</param>
        /// <returns>The property value.</returns>
        private string GetMultiLocaleString(JObject user, string propertyName)
        {
            var property = user[propertyName];
            var propertyLocalized = property["localized"];
            if (property == null || propertyLocalized == null)
            {
                return null;
            }

            var preferredLocale = property["preferredLocale"];
            string preferredLocaleKey = preferredLocale == null ? null : $"{preferredLocale.Value<string>("language")}_{preferredLocale.Value<string>("country")}";
            var values = propertyLocalized
                .Children<JProperty>()
                .ToDictionary(p => p.Name, p => p.Value.Value<string>());

            return MultiLocaleStringResolver(values, preferredLocaleKey);
        }

        private string GetFullName(JObject user)
        {
            string[] nameParts = new string[]
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

        /// <summary>
        /// The default <c>MultiLocaleString</c> resolver.
        /// Resolve it in this order:
        /// 1. Returns the <c>preferredLocale</c> value if it is set and has a value.
        /// 2. Returns the value corresponding to the <see cref="Thread.CurrentUICulture"/> if it exists.
        /// 3. Returns the first value.
        /// </summary>
        /// <param name="localizedValues">The localized values with culture keys.</param>
        /// <param name="preferredLocale">The preferred locale, if provided by LinkedIn.</param>
        /// <returns>The localized value.</returns>
        private static string DefaultMultiLocaleStringResolver(IReadOnlyDictionary<string, string> localizedValues, string preferredLocale)
        {
            if (!string.IsNullOrEmpty(preferredLocale)
                && localizedValues.TryGetValue(preferredLocale, out string preferredLocaleValue))
            {
                return preferredLocaleValue;
            }

            string currentUIKey = Thread.CurrentThread.CurrentUICulture.ToString().Replace('-', '_');
            if (localizedValues.TryGetValue(currentUIKey, out string currentUIValue))
            {
                return currentUIValue;
            }

            return localizedValues.Values.FirstOrDefault();
        }
    }
}
