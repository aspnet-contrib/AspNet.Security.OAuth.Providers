# Integrating the Zendesk Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddZendesk(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Domain = "https://glowingwaffle.zendesk.com";
        });
```

## Required Additional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Domain` | `string?` | The Zendesk domain (_Account URL_) to use for authentication. | `null` |

## Optional Settings

_None._
