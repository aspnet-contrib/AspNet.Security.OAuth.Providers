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
            options.Version = new Version(19, 0);
        });
```

### Production with Public Access Type

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddKeycloak(options =>
        {
            options.AccessType = KeycloakAuthenticationAccessType.Public;
            options.ClientId = "my-client-id";
            options.Domain = "mydomain.local";
            options.Realm = "myrealm";
            options.Version = new Version(19, 0);
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
            options.Version = new Version(19, 0);
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

| Property Name | Property Type                      | Description                              | Default Value                                   |
| :------------ | :--------------------------------- | :--------------------------------------- | :---------------------------------------------- |
| `AccessType`  | `KeycloakAuthenticationAccessType` | The Keycloak client's access token type. | `KeycloakAuthenticationAccessType.Confidential` |
| `Version`     | `Version?`                         | The Keycloak server version.             | `null`                                          |
