# Integrating the WorkWeixin Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddWorkWeixin(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.AgentId = "my-agent-id";            
        });
```

## Required Additional Settings


| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `AgentId` | `string` | Gets or sets the web application ID of the licensor. | `null` |


## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `UserIdentificationEndpoint` | `string` | The address of the endpoint to use for user identification. | `WorkWeixinAuthenticationDefaults.UserIdentificationEndpoint` |
