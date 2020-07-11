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
| `Server` | [`EVEOnlineAuthenticationServer`](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.EVEOnline/EVEOnlineAuthenticationServer.cs "EVEOnlineAuthenticationServer enumeration") | The EVE Online server to use. | `EVEOnlineAuthenticationServer.Tranquility` |
