# Integrating the Keycloak Provider

## Example

### Production

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddKeycloak(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Domain = "mydomain.local";
            options.Realm = "myrealm";
        });
```

### Local Development with Docker

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddKeycloak(options =>
        {
            options.BaseAddress = new Uri("http://localhost:8080");
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Realm = "myrealm";
        });
```

## Required Additional Settings

Only one of either `BaseAddress` or `Domain` is required to be set. If both are set, `BaseAddress` takes precedence.

| Property Name | Property Type | Description                                    | Default Value |
| :------------ | :------------ | :--------------------------------------------- | :------------ |
| `BaseAddress` | `Uri?`        | The Keycloak server's base address.            | `null`        |
| `Domain`      | `string?`     | The Keycloak domain to use for authentication. | `null`        |
| `Realm`       | `string?`     | The Keycloak realm to use for authentication.  | `null`        |

## Optional Settings

_None._
