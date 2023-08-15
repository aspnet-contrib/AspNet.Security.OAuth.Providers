# Integrating the JumpCloud Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddJumpCloud(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Domain = "https://oauth.id.jumpcloud.com";
        });
```

## Required Additional Settings

_None._

## Optional Settings

_None._
