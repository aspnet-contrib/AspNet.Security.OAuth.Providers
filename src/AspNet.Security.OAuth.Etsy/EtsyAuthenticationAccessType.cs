// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
// for more information concerning the license and the contributors participating to this project.

namespace AspNet.Security.OAuth.Etsy;

public enum EtsyAuthenticationAccessType
{
    /// <summary>
    /// Public client access type aka 'private usage access' in Etsy App Registration.
    /// </summary>
    Public,

    //// <summary>
    //// Confidential client access type aka 'commercial usage access' in Etsy App Registration. // TODO: Uncomment if someone can verify that commercial usage access supports confidential clients.
    //// </summary>
    // Confidential
}
