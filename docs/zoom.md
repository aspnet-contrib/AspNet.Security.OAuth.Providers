# Integrating the Zoom Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddZoom(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
        });
```

## Required Additional Settings

_None._