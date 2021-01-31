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
| `Scope` | `ICollection<string>` | The scope(s) you want to use on Lichess, see [documentation](https://lichess.org/api#section/Authentication "List of possible scopes"). |  |
| `UserEmailsEndpoint` | `string` | The address of the endpoint exposing the email addresses associated with the logged in user. | `LichessAuthenticationDefaults.UserEmailsEndpoint` |
