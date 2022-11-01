# Integrating the Huawei Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddHuawei(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // Optionally.
            options.Scope.Add("profile");
            options.Scope.Add("email");

            // Optionally.
            options.FetchNickName = true;
        });
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `FetchNickName` | `bool` | When FetchNickName is set to false or not set, the anonymous account is returned. If the anonymous account is unavailable, the nickname is returned. When FetchNickName is set to true, the nickname is returned. If the nickname is unavailable, the anonymous account is returned. | `false` |

### Scope
Corresponding information, such as the profile picture and email address, can be obtained only if the app has the permission to obtain the information.
* `profile`   basic information of a HUAWEI ID, such as the profile picture and nickname.
* `email` email address of a HUAWEI ID.
