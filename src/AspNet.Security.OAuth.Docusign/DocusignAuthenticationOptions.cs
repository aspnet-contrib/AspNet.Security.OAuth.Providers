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
    /// Sets or retrieves a value that determines whether sandbox or production endpoints are used.
    /// The default value of this property is <see cref="DocusignAuthenticationEnvironment.Sandbox"/>.
    /// </summary>
    public DocusignAuthenticationEnvironment Environment
    {
        get
        {
            return _environment;
        }

        set
        {
            _environment = value;
            ConfigureOptions();
        }
    }

    private DocusignAuthenticationEnvironment _environment;

    public DocusignAuthenticationOptions()
    {
        ClaimsIssuer = DocusignAuthenticationDefaults.Issuer;
        CallbackPath = DocusignAuthenticationDefaults.CallbackPath;
        Environment = DocusignAuthenticationEnvironment.Sandbox;

        ClaimActions.MapCustomJson(ClaimTypes.Name, user => user.GetString("name"));
        ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetString("email"));
        ClaimActions.MapCustomJson(ClaimTypes.GivenName, user => user.GetString("given_name"));
        ClaimActions.MapCustomJson(ClaimTypes.Surname, user => user.GetString("family_name"));
    }

    public override void Validate()
    {
        base.Validate();

        if (!Enum.IsDefined(typeof(DocusignAuthenticationEnvironment), Environment))
        {
            throw new InvalidOperationException($"The {nameof(Environment)} is not supported.");
        }
    }

    private void ConfigureOptions()
    {
        switch (Environment)
        {
            case DocusignAuthenticationEnvironment.Production:
                AuthorizationEndpoint = DocusignAuthenticationConstants.Endpoints.ProductionAuthorizationEndpoint;
                TokenEndpoint = DocusignAuthenticationConstants.Endpoints.ProductionTokenEndpoint;
                UserInformationEndpoint = DocusignAuthenticationConstants.Endpoints.ProductionUserInformationEndpoint;
                break;
            case DocusignAuthenticationEnvironment.Sandbox:
                AuthorizationEndpoint = DocusignAuthenticationConstants.Endpoints.SandboxAuthorizationEndpoint;
                TokenEndpoint = DocusignAuthenticationConstants.Endpoints.SandboxTokenEndpoint;
                UserInformationEndpoint = DocusignAuthenticationConstants.Endpoints.SandboxUserInformationEndpoint;
                break;
        }
    }
}
