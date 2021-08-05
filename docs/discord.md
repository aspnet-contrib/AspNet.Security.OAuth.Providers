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
| `Prompt` | `string?` | The value to use for the `prompt` query string parameter when making HTTP requests to the authorization endpoint. | `null` |
