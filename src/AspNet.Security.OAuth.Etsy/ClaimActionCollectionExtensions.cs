/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Etsy;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for <see cref="ClaimActionCollection"/> to map Etsy API specific user claims.
/// </summary>
public static class ClaimActionCollectionExtensions
{
    /// <summary>
    /// Maps the Etsy user's profile image URL (75x75) to the <see cref="EtsyAuthenticationConstants.Claims.ImageUrl"/> claim.
    /// </summary>
    public static void MapImageClaim(this ClaimActionCollection collection)
    {
        collection.MapJsonKey(EtsyAuthenticationConstants.Claims.ImageUrl, "image_url_75x75");
    }
}
