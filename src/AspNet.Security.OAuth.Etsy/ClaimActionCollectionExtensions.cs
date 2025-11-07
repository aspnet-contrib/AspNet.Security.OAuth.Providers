/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Etsy;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace Microsoft.Extensions.DependencyInjection;

public static class ClaimActionCollectionExtensions
{
    public static void MapImageClaim(this ClaimActionCollection collection)
    {
        collection.MapJsonKey(EtsyAuthenticationConstants.Claims.ImageUrl, "image_url_75x75");
    }
}
