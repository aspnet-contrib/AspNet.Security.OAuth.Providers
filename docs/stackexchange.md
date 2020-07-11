# Integrating the StackExchange Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddStackExchange(options =>
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
| `RequestKey` | `string` | The application request key. | `""` |
| `Site` | `string` | The site the users being authenticated are registered with. | `"StackOverflow"` |
