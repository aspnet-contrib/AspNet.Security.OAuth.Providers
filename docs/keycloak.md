# Integrating the Keycloak Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
                .AddKeycloak(options =>
                {
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    options.ClientId = "my-client-id";
                    options.ClientSecret = "my-client-secret";
                    options.Domain = "http://{my-url}/auth/realms/{realm-name}";
                    options.Scope.Add("openid");
                    options.Scope.Add("email");
                    options.Scope.Add("roles");
                    options.SaveTokens = true;
                })
```

## Required Additional Settings

| Property Name | Property Type | Description                                                                         | Default Value |
| :------------ | :------------ | :---------------------------------------------------------------------------------- | :------------ |
| `Domain`      | `string?`     | The Keycloak domain to use for authentication. This is the url of realm in Keycloak | `null`        |

## Optional Settings

_None._
