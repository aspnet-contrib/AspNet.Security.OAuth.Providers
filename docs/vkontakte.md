# Integrating the Vkontakte Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddVkontakte(options =>
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
| `ApiVersion` | `string` | The API version to use. | `"5.78"` |
| `Fields` | `ISet<string>` | The [profile fields](https://vk.com/dev/fields "User object") to return from the API. | `[ "id", "first_name", "last_name", "photo_rec", "photo", "hash" ]` |
