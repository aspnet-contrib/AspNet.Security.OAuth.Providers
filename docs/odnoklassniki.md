# Integrating the Odnoklassniki Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddOdnoklassniki(options =>
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
| `PublicSecret` | `string?` | The Public App Key from the application registration email. | `null` |
