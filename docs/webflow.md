# Integrating the Webflow Provider

> [!IMPORTANT]
> When you create the OAuth application in Webflow, make sure to enable the Authorized user (Read-only) [scope](https://developers.webflow.com/v2.0.0/data/reference/scopes). If this scope is not enabled, retrieving the user information will fail, resulting in the entire authorization flow to fail.

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddWebflow(options =>
        {
            options.ClientId = configuration["Webflow:ClientId"] ?? string.Empty;
            options.ClientSecret = configuration["Webflow:ClientSecret"] ?? string.Empty;
        })
```

## Required Additional Settings

_None._

## Optional Settings

_None._
