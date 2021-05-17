# Integrating the WorkWeixin Provider

## Example

```csharp
services.AddWorkWeixin(options => /* Auth configuration */)
        .AddQQ(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.AgentId = "my-agent-id";            
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `AgentId` | `string` | Gets or sets the web application ID of the licensor. | `default!` |
| `UserIdentificationEndpoint` | `string` | The address of the endpoint to use for user identification. | `WorkWeixinAuthenticationDefaults.UserIdentificationEndpoint` |
