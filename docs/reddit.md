# Integrating the Reddit Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddReddit(options =>
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
| `UserAgent` | `string?` | The `User-Agent` string to use for Reddit API requests. | `null` |
