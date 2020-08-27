# Integrating the QQ Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddQQ(options =>
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
| `UserIdentificationEndpoint` | `string` | The address of the endpoint to use for user identification. | `QQAuthenticationDefaults.UserIdentificationEndpoint` |
