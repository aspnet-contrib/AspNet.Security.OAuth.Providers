# Integrating the PingOne Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddPingOne(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.EnvironmentId = "63e9d5c3-5bb8-462d-8f71-8e6b2592e516";
        });
```

## Required Additional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `EnvironmentId` | `string` | The PingOne EnvironmentId to use for authentication. This can be found on the `environment.properties` page of the PingOne admin portal for your account. | `null` |

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Domain` | `string?` | The PingOne domain to use for authentication. Can be a custom domain configured in PingOne or one of the following: `auth.pingone.com` for the North America region, `auth.pingone.ca` for the Canada region, `auth.pingone.eu` for the European Union region, or `auth.pingone.asia` for the Asia-Pacific region. | `auth.pingone.com` |

