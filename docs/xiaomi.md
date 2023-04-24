# Integrating the Xiaomi Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddXiaomi(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // Optionally request the other open data interface permissions, if needed.
            // See https://dev.mi.com/console/doc/detail?pId=762 for details.
            options.Scope.Add("2");
        });
```

## Required Additional Settings

_None._

## Optional Settings

_None._
