/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.PingOne;

/// <summary>
/// Defines a set of options used by <see cref="PingOneAuthenticationHandler"/>.
/// </summary>
public class PingOneAuthenticationOptions : OAuthOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PingOneAuthenticationOptions"/> class.
    /// </summary>
    public PingOneAuthenticationOptions()
    {
        ClaimsIssuer = PingOneAuthenticationDefaults.Issuer;
        CallbackPath = PingOneAuthenticationDefaults.CallbackPath;
        Domain = PingOneAuthenticationDefaults.Domain;
        EnvironmentId = string.Empty;

        Scope.Add("openid");
        Scope.Add("profile");
        Scope.Add("email");

        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
    }

    /// <summary>
    /// Gets or sets the PingOne domain to use for authentication.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="PingOneAuthenticationDefaults.Domain"/>.
    /// </remarks>
    public string Domain { get; set; }

    /// <summary>
    /// Gets or sets the PingOne EnvironmentId to use for authentication.
    /// </summary>
    public string EnvironmentId { get; set; }

    /// <inheritdoc/>
    public override void Validate()
    {
        base.Validate();

        if (!Uri.TryCreate(AuthorizationEndpoint, UriKind.Absolute, out _))
        {
            throw new ArgumentException(
                $"The '{nameof(AuthorizationEndpoint)}' option must be set to a valid URI.",
                nameof(AuthorizationEndpoint));
        }

        if (!Uri.TryCreate(TokenEndpoint, UriKind.Absolute, out _))
        {
            throw new ArgumentException(
                $"The '{nameof(TokenEndpoint)}' option must be set to a valid URI.",
                nameof(TokenEndpoint));
        }

        if (!Uri.TryCreate(UserInformationEndpoint, UriKind.Absolute, out _))
        {
            throw new ArgumentException(
                $"The '{nameof(UserInformationEndpoint)}' option must be set to a valid URI.",
                nameof(UserInformationEndpoint));
        }
    }
}
