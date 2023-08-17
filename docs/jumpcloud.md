# Integrating the JumpCloud Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddJumpCloud(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Domain = "oauth.id.jumpcloud.com";
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Domain` | `string?` | The JumpCloud domain to use for authentication. | `"oauth.id.jumpcloud.com"` |
