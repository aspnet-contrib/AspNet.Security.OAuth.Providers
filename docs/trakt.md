# Integrating the Trakt Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddTrakt(options =>
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
| `ApiVersion` | `string` | The Trakt API version to use. | `"2"` |
