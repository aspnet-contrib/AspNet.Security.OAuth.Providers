# Integrating the Linear Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddLinear(options =>
        {
            options.ClientId = configuration["Linear:ClientId"] ?? string.Empty;
            options.ClientSecret = configuration["Linear:ClientSecret"] ?? string.Empty;

            // 'read' scope is added by default. Add additional scopes required
            // options.Scope.Add("write");

            // Additional authorization parameters can also be added
            // options.AdditionalAuthorizationParameters.Add("prompt", "consent");
        })
```

## Required Additional Settings

_None._

## Optional Settings

_None._
