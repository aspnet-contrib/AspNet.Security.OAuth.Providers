# Integrating the Kroger Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddKroger(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // Optionally request other permissions
            // See https://developer.kroger.com/reference/#section/Authentication for details.
            options.Scope.Add("product.compact");
            options.Scope.Add("cart.basic:write");         
        });
```

## Required Additional Settings

_None._

## Optional Settings

_None._