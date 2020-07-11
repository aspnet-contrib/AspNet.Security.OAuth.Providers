# Integrating the SuperOffice Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddSuperOffice(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Environment = SuperOfficeAuthenticationEnvironment.Production;
        });
```

## Required Additional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Environment` | `SuperOfficeAuthenticationEnvironment` | The target online environment. | `SuperOfficeAuthenticationEnvironment.Development` |

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Authority` | `string` | The Authority to use when making OpenId Connect calls. | The authority associated with the value of `Environment`. |
| `ConfigurationManager` | `IConfigurationManager<OpenIdConnectConfiguration>?` | The configuration manager to use for the OpenId configuration. | `null` |
| `Environment` | `SuperOfficeAuthenticationEnvironment` | The target online environment. | `SuperOfficeAuthenticationEnvironment.Development` |
| `IncludeFunctionalRightsAsClaims` | `bool` | Whether to include functional rights as claims. | `false` |
| `IncludeIdTokenAsClaims` | `bool` | Whether to include the ID token as a claim. | `false` |
| `MetadataAddress` | `string` | The URL to use to get the OpenId Connect configuration. | The metadata address associated with the value of `Environment`. |
| `SecurityTokenHandler` | `JsonWebTokenHandler?` | The JSON Web Token handler to use. | `null` |
| `TokenValidationParameters` | `TokenValidationParameters` | The JSON Web Token validation parameter to use. | `new TokenValidationParameters()` |
