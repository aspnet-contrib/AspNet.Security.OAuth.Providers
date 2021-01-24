# Integrating the Lichess Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddLichess(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
			
			options.Scope.Clear();
            options.Scope.Add(LichessAuthenticationConstants.Scopes.EmailRead);
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Scope` | `string` | The scope you want to use on lichess, see [documentation](https://lichess.org/api#section/Authentication "List of possible scopes"). |  |
| `UserEmailsEndpoint` | `string` | The address of the endpoint exposing the email addresses associated with the logged in user. | `LichessAuthenticationDefaults.UserEmailsEndpoint` |
