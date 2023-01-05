# Integrating the KOOK Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddKook(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // The scope get_user_info is added by default.
            // Please make sure the scope get_user_info is enabled in your KOOK developer center.
            // If you do not want to use the default scope indeed, remove it as shown below:
            // options.Scope.Remove("get_user_info");
        });
```

## Required Additional Settings

_None._

## Optional Settings

_None._
