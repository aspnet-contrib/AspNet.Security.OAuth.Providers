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

    /// <summary>
    /// Gets the list of fields to retrieve from the user information endpoint.
    /// See <a>https://docs.microsoft.com/en-us/linkedin/consumer/integrations/self-serve/sign-in-with-linkedin</a> for more information.
    /// </summary>
    [Obsolete("This property is no longer used and will be removed in a future version.", false)]
    public ISet<string> Fields { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ProfileFields.Id,
            ProfileFields.GivenName,
            ProfileFields.FamilyName,
            EmailAddressField
        };

    /// <summary>
    /// Gets or sets a <c>MultiLocaleString</c> resolver, a function which takes all localized values
    /// and an eventual preferred locale from the member and returns the selected localized value.
    /// The default implementation resolve it in this order:
    /// 1. Returns the <c>preferredLocale</c> value if it is set and has a value.
    /// 2. Returns the value corresponding to the <see cref="Thread.CurrentUICulture"/> if it exists.
    /// 3. Returns the first value.
    /// </summary>
    /// <see cref="DefaultMultiLocaleStringResolver(IReadOnlyDictionary{string, string}, string?)"/>
    [Obsolete("This method is no longer used and will be removed in a future version.", false)]
    public Func<IReadOnlyDictionary<string, string?>, string?, string?> MultiLocaleStringResolver { get; set; } = DefaultMultiLocaleStringResolver;

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
    private static string? DefaultMultiLocaleStringResolver(IReadOnlyDictionary<string, string?> localizedValues, string? preferredLocale)
    {
        if (!string.IsNullOrEmpty(preferredLocale) &&
            localizedValues.TryGetValue(preferredLocale, out var preferredLocaleValue))
        {
            return preferredLocaleValue;
        }

        var currentUIKey = Thread.CurrentThread.CurrentUICulture.ToString().Replace('-', '_');
        if (localizedValues.TryGetValue(currentUIKey, out var currentUIValue))
        {
            return currentUIValue;
        }

        return localizedValues.Values.FirstOrDefault();
    }
}
