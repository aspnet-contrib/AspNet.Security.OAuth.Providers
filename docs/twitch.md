# Integrating the Twitch Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddTwitch(options =>
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
| `ForceVerify` | `bool` | Whether to send the `force_verify=true` query string parameter with authenticating users. | `false` |
