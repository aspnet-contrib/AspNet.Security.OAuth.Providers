# Integrating the EVE Online Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddEVEOnline(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Server` | `EVEOnlineAuthenticationServer` | The EVE Online server to use. | `EVEOnlineAuthenticationServer.Tranquility` |
