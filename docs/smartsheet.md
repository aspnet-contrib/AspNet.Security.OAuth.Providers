# Integrating the Smartsheet Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddSmartsheet(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // At least one scope must be passed
            options.Scope.Add("READ_SHEETS");
        });
```

## Required Additional Settings

_None._

## Optional Settings

_None._
