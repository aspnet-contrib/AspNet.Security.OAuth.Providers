/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Apple;

/// <summary>
/// Contains information about the current request.
/// </summary>
/// <remarks>
/// Creates a new instance of the <see cref="AppleGenerateClientSecretContext"/> class.
/// </remarks>
/// <param name="context">The HTTP context.</param>
/// <param name="scheme">The authentication scheme.</param>
/// <param name="options">The authentication options associated with the scheme.</param>
public class AppleGenerateClientSecretContext(
    HttpContext context,
    AuthenticationScheme scheme,
    AppleAuthenticationOptions options) : BaseContext<AppleAuthenticationOptions>(context, scheme, options)
{
}
