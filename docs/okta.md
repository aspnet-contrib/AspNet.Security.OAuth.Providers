# Integrating the Okta Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddOkta(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Domain = "https://dev-000000.okta.com";
        });
```

## Required Additional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Domain` | `string?` | The Okta domain (_Org URL_) to use for authentication. This can be found on the `/dev/console` page of the Okta admin portal for your account. | `null` |

## Optional Settings

_None._
