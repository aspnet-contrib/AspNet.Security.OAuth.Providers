# Integrating the VK ID Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddVkId(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
        });
```

## Required Additional Settings

For confidential VK ID applications, `ServiceToken` is required.

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `ServiceToken` | `string` | The service access token required by confidential VK ID applications. It is sent to the VK ID token endpoint as `service_token`. | `string.Empty` |

For example:

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddVkId(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.ServiceToken = "my-service-token";
        });
```
