# Integrating the Etsy Provider

Etsy's OAuth implementation uses Authorization Code with **PKCE** and issues **refresh tokens**.

This provider enables PKCE by default and validates scopes to match Etsy's requirements.

- [Integrating the Etsy Provider](#integrating-the-etsy-provider)
  - [Example](#example)
  - [Required Additional Settings](#required-additional-settings)
  - [Optional Settings](#optional-settings)
  - [Quick Links](#quick-links)

## Example

```csharp
using AspNet.Security.OAuth.Etsy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddAuthentication(options => { /* Authentication options */ })
  .AddEtsy(options =>
  {
    options.ClientId = "my-etsy-client-id";
    options.ClientSecret = "my-etsy-client-secret"; // Optional as Etsy requires PKCE
    options.IncludeDetailedUserInfo = true; // Optional to get first name, last name, email claims
    options.ClaimActions.MapImageClaim(); // Optional Extension to map the image_url_75x75 claim, will not be mapped automatically
  });
```

## Required Additional Settings

- You can obtain the Client ID (`keystring`) for your app by registering your application on [Etsy's developer portal](https://www.etsy.com/developers/your-apps).
- The ClientSecret (`shared secret` in the Etsy app details) is optional for public clients using PKCE.

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `IncludeDetailedUserInfo` | `bool` | Fetch extended profile data with auto-mapped claims (Email, GivenName, Surname). | `false` |
| `ClaimActions.MapImageClaim()` | Extension method | Map the `image_url_75x75` claim to `EtsyAuthenticationConstants.Claims.ImageUrl`. | Not mapped automatically |
| `DetailedUserInfoEndpoint` | `string` | Endpoint to retrieve detailed user information. | `https://openapi.etsy.com/v3/application/users/` |

Additional helpers are available via `EtsyAuthenticationConstants.Scopes.*` for Etsy OAuth scopes and `EtsyAuthenticationConstants.Claims.*` for claim type constants used for the `getMe` and `getUser` endpoints.

## Quick Links

| Resource | Link |
|:--|:--|
| Register your App on Etsy: | [Apps You've Made](https://www.etsy.com/developers/your-apps)  |
| Official Etsy Authentication API Documentation: | [Etsy Developer Documentation](https://developers.etsy.com/documentation/essentials/authentication) |
| Requesting a Refresh OAuth Token: | [Etsy Refresh Token Guide](https://developers.etsy.com/documentation/essentials/authentication#requesting-a-refresh-oauth-token) |
| Etsy API Reference: | [Etsy API Reference](https://developers.etsy.com/documentation/reference) |
