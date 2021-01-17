# Integrating the amoCRM Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddAmoCrm(options =>
        {
            options.Account = "my-account";
            options.ClientId = "client-id";
            options.ClientSecret = "client-secret";
        });
```

## Required Additional Settings

_None._

## Optional Settings

_None._
