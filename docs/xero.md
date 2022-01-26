# Integrating the Xero Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddXero(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // Optionally specify additional scopes, if needed
            options.Scope.Add("profile");
            options.Scope.Add("email");
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `ConfigurationManager` | `IConfigurationManager<OpenIdConnectConfiguration>?` | The configuration manager to use for the OpenId configuration. | `null` |
| `SecurityTokenHandler` | `JsonWebTokenHandler?` | The JSON Web Token handler to use. | `null` |
| `TokenValidationParameters` | `TokenValidationParameters` | The JSON Web Token validation parameter to use. | `new TokenValidationParameters()` |