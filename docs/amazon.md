# Integrating the Amazon Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddAmazon(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // Optionally request the user's postal code, if needed
            options.Scope.Add("postal_code");
            options.Fields.Add("postal_code");
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:-:|:-:|:-:|:-:|
| `Fields` | `ISet<string>` | The fields of the user's profile to return. | `[ "email", "name", "user_id" ]` |
