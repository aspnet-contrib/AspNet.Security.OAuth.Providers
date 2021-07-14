# Integrating the Dropbox Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddDropbox(options =>
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
| `AccessType` | `string?` | Sets the _token_access_type_ parameter which defines the token response.  Accepted values are `online`, `offline`, `legacy` | `null` |

### AccessType

* `online` The response will contain only a short-lived _access_token_. 

* `offline` The response will contain a short-lived _access_token_ and a long-lived _refresh_token_ that can be used to request a new short-lived access token as long as a user's approval remains valid. 

* `legacy` The response will default to returning a long-lived _access_token_ if they are allowed in the app console. If long-lived access tokens are disabled in the app console, this parameter defaults to `online`.
