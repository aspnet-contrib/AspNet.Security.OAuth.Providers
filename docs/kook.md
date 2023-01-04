# Integrating the KOOK Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddKook(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // The scope get_user_info is added by default
            // Please make sure the scope get_user_info is enabled in your KOOK developer center
            // Hence, whether to add the scope get_user_info here manually is optional
            // options.Scope.Add("get_user_info");
        });
```

## Required Additional Settings

_None._

## Optional Settings

_None._
