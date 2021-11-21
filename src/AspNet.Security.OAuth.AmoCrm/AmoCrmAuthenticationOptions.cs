/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using System.Security.Claims;

namespace AspNet.Security.OAuth.AmoCrm;

/// <summary>
/// Defines a set of options used by <see cref="AmoCrmAuthenticationHandler"/>.
/// </summary>
public class AmoCrmAuthenticationOptions : OAuthOptions
{
    private string _account = "example";

    /// <summary>
    /// Initializes a new instance of the <see cref="AmoCrmAuthenticationOptions"/> class.
    /// </summary>
    public AmoCrmAuthenticationOptions()
    {
        ClaimsIssuer = AmoCrmAuthenticationDefaults.Issuer;
        CallbackPath = AmoCrmAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = AmoCrmAuthenticationDefaults.AuthorizationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
    }

    /// <summary>
    /// Gets or sets the amoCRM account name.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is null or empty string.</exception>
    public string Account
    {
        get => _account;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Account cannot be null or white space.", nameof(value));
            }

            _account = value;
            TokenEndpoint = string.Format(CultureInfo.InvariantCulture, AmoCrmAuthenticationDefaults.TokenEndpointFormat, value);
            UserInformationEndpoint = string.Format(CultureInfo.InvariantCulture, AmoCrmAuthenticationDefaults.UserInformationEndpointFormat, value);
        }
    }
}
