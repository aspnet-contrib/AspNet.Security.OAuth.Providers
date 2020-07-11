# Integrating the Gitee Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddGitee(options =>
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
| `UserEmailsEndpoint` | `string` | The address of the endpoint exposing the email addresses associated with the logged in user. | `GiteeAuthenticationDefaults.UserEmailsEndpoint` |
