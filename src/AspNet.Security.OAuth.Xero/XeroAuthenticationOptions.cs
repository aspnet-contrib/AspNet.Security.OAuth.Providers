/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Xero;

/// <summary>
/// Defines a set of options used by <see cref="XeroAuthenticationHandler"/>.
/// </summary>
public class XeroAuthenticationOptions : OAuthOptions
{
    /// <summary>
    /// Initializes a new <see cref="XeroAuthenticationOptions"/>
    /// </summary>
    public XeroAuthenticationOptions()
    {
        AuthorizationEndpoint = XeroAuthenticationDefaults.AuthorizeEndpoint;
        TokenEndpoint = XeroAuthenticationDefaults.TokenEndpoint;
        MetadataAddress = XeroAuthenticationDefaults.MetadataEndpoint;

        CallbackPath = XeroAuthenticationDefaults.CallbackPath;
        ClaimsIssuer = XeroAuthenticationDefaults.ClaimsIssuer;

        Scope.Add("openid");
    }

    /// <summary>
    /// Gets the URI the middleware uses to obtain the OpenId Connect configuration.
    /// </summary>
    public string MetadataAddress { get; internal set; }

    /// <summary>
    /// Gets or sets the security token validator to use.
    /// </summary>
    public JsonWebTokenHandler? SecurityTokenHandler { get; set; }

    /// <summary>
    /// Gets or sets the parameters used to validate identity tokens.
    /// </summary>
    /// <remarks>Contains the types and definitions required for validating a token.</remarks>
    public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();

    /// <summary>
    /// Gets or sets the configuration manager responsible for retrieving, caching, and refreshing the
    /// OpenId configuration from metadata. If not provided, then one will be created using the
    /// and <see cref="RemoteAuthenticationOptions.Backchannel"/> properties.
    /// </summary>
    public IConfigurationManager<OpenIdConnectConfiguration>? ConfigurationManager { get; set; }
}
