# Integrating the BungieNet

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddBungieNet(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.ApiKey = "my-api-key";
        });
```

## Required Additional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `ApiKey` | `string` | When you have registered an Application at https://www.bungie.net/en/Application, you will receive an API key. You should pass it in via this header with every request. | `null` |

## Optional Settings

_None._
