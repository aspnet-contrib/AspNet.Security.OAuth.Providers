/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Docusign;

/// <summary>
/// Defines a set of options used by <see cref="DocusignAuthenticationHandler"/>.
/// </summary>
public class DocusignAuthenticationOptions : OAuthOptions
{
    /// <summary>
    /// Gets or sets a value that determines whether development or production endpoints are used.
    /// The default value of this property is <see cref="DocusignAuthenticationEnvironment.Production"/>.
    /// </summary>
    public DocusignAuthenticationEnvironment Environment { get; set; }

    public DocusignAuthenticationOptions()
    {
        ClaimsIssuer = DocusignAuthenticationDefaults.Issuer;
        CallbackPath = DocusignAuthenticationDefaults.CallbackPath;
        Environment = DocusignAuthenticationEnvironment.Production;

        ClaimActions.MapCustomJson(ClaimTypes.Name, user => user.GetString("name"));
        ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetString("email"));
        ClaimActions.MapCustomJson(ClaimTypes.GivenName, user => user.GetString("given_name"));
        ClaimActions.MapCustomJson(ClaimTypes.Surname, user => user.GetString("family_name"));
    }
}
