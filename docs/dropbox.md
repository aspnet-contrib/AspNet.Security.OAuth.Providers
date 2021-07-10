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
| `ResponseType` | `string?` | Sets the _response_type_ parameter, signifying the grant type requested, either `token` or `code`. | `null` |

### AccessType

* `online` The response will contain only a short-lived _access_token_. 

* `offline` The response will contain a short-lived _access_token_ and a long-lived _refresh_token_ that can be used to request a new short-lived access token as long as a user's approval remains valid. 

* `legacy` The response will default to returning a long-lived _access_token_ if they are allowed in the app console. If long-lived access tokens are disabled in the app console, this parameter defaults to `online`.

### ResponseType

* _(Recommended)_ The `code` flow returns a code via the _redirect_uri_ callback which should then be converted into a bearer token using the /oauth2/token call. This is the recommended flow for apps that are running on a server.

  The PKCE flow is an extension of the code flow that uses dynamically generated codes instead of a secret to perform an OAuth exchange from public clients. The PKCE flow is a newer, more secure alternative to the token (implicit) flow. It is the recommended flow for client-side apps, such as mobile or JavaScript apps.

* _(Legacy)_ The `token` or implicit grant flow returns the bearer token via the _redirect_uri_ callback, rather than requiring your app to make a second call to a server. This is useful for pure client-side apps, such as mobile apps or JavaScript-based apps.