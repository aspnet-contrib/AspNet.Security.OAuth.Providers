# Integrating the Miro Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddMiro(options =>
        {
            options.ClientId = configuration["Miro:ClientId"] ?? string.Empty;
            options.ClientSecret = configuration["Miro:ClientSecret"] ?? string.Empty;
        })
```

## Required Additional Settings

_None._

## Optional Settings

_None._

## Retrieving the user's email address

The Miro provider does not return the user's email address, since the API endpoints to do that is available only on the Miro Enterprise plan. If you are on the Miro Enterprise plan, you can use the access token returned by the authentication flow to retrieve the user's email address yourself by following the Miro documentation for [getting the user info and email](https://developers.miro.com/docs/get-user-info-and-email).

