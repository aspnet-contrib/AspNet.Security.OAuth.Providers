# Integrating the Patreon Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddPatreon(options =>
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
| `Fields` | `ISet<string>` | The list of fields to retrieve from the user information endpoint. | `[ "first_name", "full_name", "last_name", "thumb_url", "url" ]` |
| `Includes` | `ISet<string>` | The list of related data to include from the user information endpoint. | `[]` |
