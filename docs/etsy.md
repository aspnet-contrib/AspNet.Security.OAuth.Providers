# Integrating the Etsy Provider

Etsy's OAuth implementation uses Authorization Code with PKCE and issues refresh tokens. This provider enables PKCE by default and validates scopes to match Etsy's requirements.

## Quick Links

- Register your App at [Apps You've Made](https://www.etsy.com/developers/your-apps) on Etsy.
- Official Etsy Authentication API Documentation: [Etsy Developer Documentation](https://developers.etsy.com/documentation/essentials/authentication)
- Requesting a Refresh OAuth Token: [Etsy Refresh Token Guide](https://developers.etsy.com/documentation/essentials/authentication#requesting-a-refresh-oauth-token)
- Etsy API Reference: [Etsy API Reference](https://developers.etsy.com/documentation/reference)

## Quick start

Add the Etsy provider in your authentication configuration and request any additional scopes you need ("shops_r" is added by default):

```csharp
services.AddAuthentication(options => /* Auth configuration */)
    .AddEtsy(options =>
    {
      options.ClientId = builder.Configuration["Etsy:ClientId"]!;

      // Optional: request additional scopes
      options.Scope.Add(AspNet.Security.OAuth.Etsy.EtsyAuthenticationConstants.Scopes.ListingsRead);

      // Optional: fetch extended profile (requires email_r)
      // options.IncludeDetailedUserInfo = true;
      // options.Scope.Add(AspNet.Security.OAuth.Etsy.EtsyAuthenticationConstants.Scopes.EmailRead);
    });
```

## Required Additional Settings

_None._

> [!NOTE]
>
> - ClientSecret is optional for apps registered with Personal Access (public client); Etsy's flow uses Authorization Code with PKCE.
> - PKCE is required and is enabled by default.
> - The default callback path is `/signin-etsy`.
> - Etsy requires at least one scope; `shops_r` must always be included and is added by default.
> - To call the [`getUser` endpoint](https://developers.etsy.com/documentation/reference/#operation/getUser) or when `IncludeDetailedUserInfo` is enabled, add `email_r`.

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Scope` | `ICollection<string>` | Scopes to request. At least one scope is required and `shops_r` must be included (it is added by default). Add `email_r` if you enable `IncludeDetailedUserInfo`. | `["shops_r"]` |
| `IncludeDetailedUserInfo` | `bool` | Makes a second API call to fetch extended profile data (requires `email_r`). | `false` |
| `AccessType` | `EtsyAuthenticationAccessType` | Apps registered as `Personal Access` don't require the client secret in [Authorization Code Flow](https://datatracker.ietf.org/doc/html/rfc6749#section-4.1). | `Personal` |
| `SaveTokens` | `bool` | Persists access/refresh tokens (required by Etsy and validated). | `true` |

### Scope constants

Use `EtsyAuthenticationConstants.Scopes.*` instead of string literals. Common values:

- `EmailRead` → `email_r`
- `ListingsRead` → `listings_r`
- `ListingsWrite` → `listings_w`
- `ShopsRead` → `shops_r`
- `TransactionsRead` → `transactions_r`

## Validation behavior

- PKCE and token saving are required and enforced by the options validator.
- Validation fails if no scopes are requested or if `shops_r` is missing.
- If `IncludeDetailedUserInfo` is true, `email_r` must be present.

## Refreshing tokens

This provider saves tokens by default (`SaveTokens = true`). Etsy issues a refresh token; you are responsible for performing the refresh flow using the saved token when the access token expires.

```csharp
var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
```

See [Requesting a Refresh OAuth Token](#quick-links) in the Quick Links above for the HTTP details.

## Claims

Basic claims are populated from `/v3/application/users/me`. When `IncludeDetailedUserInfo` is enabled and `email_r` is granted, additional claims are populated from `/v3/application/users/{user_id}`.

| Claim Type | Value Source | Description |
|:--|:--|:--|
| `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier` | `user_id` | Primary user identifier |
| `urn:etsy:user_id` | `user_id` | Etsy-specific user ID claim (in addition to NameIdentifier) |
| `urn:etsy:shop_id` | `shop_id` | User's shop ID |
| `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress` | `primary_email` | Primary email address |
| `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname` | `first_name` | First name |
| `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname` | `last_name` | Last name |
| `urn:etsy:primary_email` | `primary_email` | Etsy-specific email claim |
| `urn:etsy:first_name` | `first_name` | Etsy-specific first name claim |
| `urn:etsy:last_name` | `last_name` | Etsy-specific last name claim |
| `urn:etsy:image_url` | `image_url_75x75` | 75x75 profile image URL |

## Configuration

### Minimal configuration

#### [Program.cs](#tab/minimal-configuration-program)

```csharp
using AspNet.Security.OAuth.Etsy;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = EtsyAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddEtsy(options =>
{
    options.ClientId = builder.Configuration["Etsy:ClientId"]!;

  // Enable extended profile (requires email_r)
  // options.IncludeDetailedUserInfo = true;
  // options.Scope.Add(EtsyAuthenticationConstants.Scopes.EmailRead);

  // Add other optional scopes (shops_r is added by default)
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
```

#### [appsettings.json or appsettings.Development.json](#tab/minimal-configuration-appsettings)

```json
{
  "Etsy": {
    "ClientId": "your-etsy-api-key"
  }
}
```

***

### Advanced using App Settings

You can keep using code-based configuration, or bind from configuration values. Here is a comprehensive `appsettings.json` example covering supported options and common scopes:

```json
{
  "Etsy": {
    "ClientId": "your-etsy-api-key",
    "AccessType": "Personal",
    "IncludeDetailedUserInfo": true,
    "SaveTokens": true,
    "Scopes": [ "shops_r", "email_r" ]
  },
  "Logging": {
    "LogLevel": { "Default": "Information" }
  }
}
```

If you bind from configuration, set the options in code, for example:

```csharp
.AddEtsy(options =>
{
    var section = builder.Configuration.GetSection("Etsy");
    options.ClientId = section["ClientId"]!;
    options.AccessType = Enum.Parse<EtsyAuthenticationAccessType>(section["AccessType"] ?? "Personal", true);
    options.IncludeDetailedUserInfo = bool.TryParse(section["IncludeDetailedUserInfo"], out var detailed) && detailed;
    options.SaveTokens = !bool.TryParse(section["SaveTokens"], out var save) || save; // defaults to true

    // Apply scopes from config if present
    var scopes = section.GetSection("Scopes").Get<string[]?>();
    if (scopes is { Length: > 0 })
    {
        foreach (var scope in scopes)
        {
            options.Scope.Add(scope);
        }
    }
})
```

> [!NOTE]
> Make sure to use proper [Secret Management for production applications](https://learn.microsoft.com/aspnet/core/security/app-secrets).

## Accessing claims

**Using Minimal API:**

```csharp
using AspNet.Security.OAuth.Etsy;
using System.Security.Claims;

app.MapGet("/profile", (ClaimsPrincipal user) =>
{
  var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
  var shopId = user.FindFirstValue(EtsyAuthenticationConstants.Claims.ShopId);
  var email = user.FindFirstValue(ClaimTypes.Email);
  var firstName = user.FindFirstValue(ClaimTypes.GivenName);
  var lastName = user.FindFirstValue(ClaimTypes.Surname);
  var imageUrl = user.FindFirstValue(EtsyAuthenticationConstants.Claims.ImageUrl);

  return Results.Ok(new { userId, shopId, email, firstName, lastName, imageUrl });
}).RequireAuthorization();
```

## Feature-style typed Minimal API endpoints with MapGroup

```csharp
using AspNet.Security.OAuth.Etsy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace MyApi.Features.Authorization;

public static class EtsyAuthEndpoints
{
  public static IEndpointRouteBuilder MapEtsyAuth(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/etsy")
      .WithTags("Etsy Authentication");

    // Sign-in: triggers the Etsy OAuth handler
    group.MapGet("/signin", SignInAsync)
      .WithName("EtsySignIn")
      .WithSummary("Initiate Etsy OAuth authentication");

    // Sign-out: removes the auth cookie/session
    group.MapGet("/signout", SignOutAsync)
      .WithName("EtsySignOut")
      .WithSummary("Sign out from Etsy authentication");

    // Protected: returns the authenticated user's profile
    group.MapGet("/user-info", GetProfileAsync)
      .RequireAuthorization()
      .WithName("User Info")
      .WithSummary("Get authenticated user's information");

    // Protected: returns saved OAuth tokens
    group.MapGet("/tokens", GetTokensAsync)
      .RequireAuthorization()
      .WithName("EtsyTokens")
      .WithSummary("Get OAuth access and refresh tokens");

    return app;
  }

  private static Results<ChallengeHttpResult, RedirectHttpResult> SignInAsync(string? returnUrl)
    => TypedResults.Challenge(
      new AuthenticationProperties { RedirectUri = returnUrl ?? "/" },
      new[] { EtsyAuthenticationDefaults.AuthenticationScheme });

  private static async Task<RedirectHttpResult> SignOutAsync(HttpContext context)
  {
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return TypedResults.Redirect("/");
  }

  private static Task<Ok<UserInfo>> GetProfileAsync(ClaimsPrincipal user)
  {
    var profile = new UserInfo
    {
      UserId = user.FindFirstValue(ClaimTypes.NameIdentifier)!,
      ShopId = user.FindFirstValue(EtsyAuthenticationConstants.Claims.ShopId)!,
      Email = user.FindFirstValue(ClaimTypes.Email),
      FirstName = user.FindFirstValue(ClaimTypes.GivenName),
      LastName = user.FindFirstValue(ClaimTypes.Surname),
      ImageUrl = user.FindFirstValue(EtsyAuthenticationConstants.Claims.ImageUrl)
    };

    return Task.FromResult(TypedResults.Ok(profile));
  }

  private static async Task<Ok<TokenInfo>> GetTokensAsync(HttpContext context)
  {
    var tokenInfo = new TokenInfo
    {
      AccessToken = await context.GetTokenAsync("access_token"),
      RefreshToken = await context.GetTokenAsync("refresh_token"),
      ExpiresAt = await context.GetTokenAsync("expires_at")
    };

    return TypedResults.Ok(tokenInfo);
  }

  public sealed record UserInfo
  {
    public required string UserId { get; init; }
    public required string ShopId { get; init; }
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? ImageUrl { get; init; }
  }

  public sealed record TokenInfo
  {
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
    public string? ExpiresAt { get; init; }
  }
}
```
