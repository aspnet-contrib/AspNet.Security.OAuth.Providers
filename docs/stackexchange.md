# Integrating the StackExchange Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddStackExchange(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.RequestKey = "my-request-key";
        });
```

## Required Additional Settings

| Property Name | Property Type | Description |
|:--|:--|:--|:--|
| `RequestKey` | `string` | The application request key. |

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Site` | `string` | The site the users being authenticated are registered with. | `"StackOverflow"` |
