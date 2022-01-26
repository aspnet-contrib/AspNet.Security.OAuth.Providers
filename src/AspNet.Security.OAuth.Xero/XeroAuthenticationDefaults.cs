/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Xero;

/// <summary>
/// Default values used by the Xero authentication middleware.
/// </summary>
public static class XeroAuthenticationDefaults
{
    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "Xero";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public const string CallbackPath = "/signin-xero";

    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public const string DisplayName = "Xero";

    /// <summary>
    /// A format string used to construct <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public const string ClaimsIssuer = "https://identity.xero.com";

    /// <summary>
    /// A format string used to populate OAuth authorize endpoint.
    /// </summary>
    public const string AuthorizeEndpoint = "https://login.xero.com/identity/connect/authorize";

    /// <summary>
    /// A format string used to populate OAuth token endpoint.
    /// </summary>
    public const string TokenEndpoint = "https://identity.xero.com/connect/token";

    /// <summary>
    /// A format string used to construct well-known configuration endpoint.
    /// </summary>
    public const string MetadataEndpoint = "https://identity.xero.com/.well-known/openid-configuration";
}
