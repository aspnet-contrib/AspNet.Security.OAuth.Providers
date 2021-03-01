# Integrating the Line Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddLine(options =>
        {
            options.ClientId = "my-channel-id";
            options.ClientSecret = "my-channel-secret";
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Prompt` | `bool` | Used to force the consent screen to be displayed even if the user has already granted all requested permissions. | `false` |
| `UserEmailsEndpoint` | `string` | The address of the endpoint exposing the email addresses associated with the logged in user. | `LineAuthenticationDefaults.UserEmailsEndpoint` |
