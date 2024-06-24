# Integrating the Zoho Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddZoho(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Region = ZohoAuthenticationRegion.Global;
        });
```

## Required Additional Settings

| Property Name | Property Type                                                                                                                                                                                                       | Description                                       | Default Value                     |
|:--------------|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:--------------------------------------------------|:----------------------------------|
| `Region`      | [`ZohoAuthenticationRegion`](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.Zoho/ZohoAuthenticationRegion.cs "ZohoAuthenticationRegion enumeration") | The target online region for Zoho authentication. | `ZohoAuthenticationRegion.Global` |

## Optional Settings

_None._
