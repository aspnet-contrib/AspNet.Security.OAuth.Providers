# Integrating the Kloudless Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddKloudless(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            options.Scope.Clear();
            options.Scope.Add(KloudlessAuthenticationConstants.Scopes.Calendar);
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Scope` | `ICollection<string>` | The scope(s) you want to use on Kloudless, see [documentation](https://developers.kloudless.com/guides/kb/scopes.html "List of possible scopes"). | `KloudlessAuthenticationConstants.Scopes.Any` |
