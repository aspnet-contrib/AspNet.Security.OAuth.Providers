# Integrating the eBay Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddEbay(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.RuName = "my-ru-name";
        });
```

## Required Additional Settings

| Property Name | Property Type | Description                                                  | Default Value |
| :------------ | :------------ | :----------------------------------------------------------- | :------------ |
| `RuName`      | `string?`     | Sets the `redirect_uri` parameter that the service uses for authorization, as well as a URL to redirect the user to after they've complete the permissions grant request. Instead of a URL, the eBay services require a custom *RuName* value that eBay generates and assigns to your application. For further info see also the official [documentation](https://developer.ebay.com/api-docs/static/oauth-redirect-uri.html). | `default!` |

## Optional Settings

None.
