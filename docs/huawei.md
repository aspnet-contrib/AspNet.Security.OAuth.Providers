# Integrating the Huawei Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddHuawei(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // Optionally return the user's profile and email address
            options.Scope.Add("profile");
            options.Scope.Add("email");

            // Optionally get the user's nickname
            options.FetchNickname = true;
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `FetchNickname` | `bool` | When `false` the anonymous account is returned. If the anonymous account is unavailable, the nickname is returned. When `true`, the nickname is returned. If the nickname is unavailable, the anonymous account is returned. | `false` |
