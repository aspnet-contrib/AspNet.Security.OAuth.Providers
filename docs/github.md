# Integrating the GitHub Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddGitHub(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // Optional domain name for GitHub Enterprise on-premises deployments
            options.EnterpriseDomain = "github.corp.local";
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `EnterpriseDomain` | `string?` | The domain name to use for a GitHub Enterprise on-premises deployment. | `null` |
| `UserEmailsEndpoint` | `string` | The address of the endpoint exposing the email addresses associated with the logged in user. | `GitHubAuthenticationDefaults.UserEmailsEndpoint` |
