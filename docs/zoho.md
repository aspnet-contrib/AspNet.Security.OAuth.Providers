# Integrating the Zoho Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddZoho(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type                                                                                                                                                                                                       | Description                                       | Default Value                     |
|:--------------|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:--------------------------------------------------|:----------------------------------|
| `Region`      | [`ZohoAuthenticationRegion`](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.Zoho/ZohoAuthenticationRegion.cs "ZohoAuthenticationRegion enumeration") | The target online region for Zoho authentication. | `ZohoAuthenticationRegion.Global` |
