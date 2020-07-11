# Integrating the Discord Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddDiscord(options =>
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
| `DiscordAvatarFormat` | `string` | Gets or sets the URL format string to use for user avatar images. | `DiscordAuthenticationConstants.Urls.AvatarUrlFormat` |
| `DiscordCdn` | `string` | The URL to use for the Discord CDN. | `DiscordAuthenticationConstants.Urls.DiscordCdn` |
| `Prompt` | `string?` | The value to use for the `prompt` query string parameter when making HTTP requests to the authorization endpoint. | `null` |
