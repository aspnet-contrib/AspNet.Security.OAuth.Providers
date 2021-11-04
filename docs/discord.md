# Integrating the Discord Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddDiscord(options =>
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
| `Prompt` | `string?` | The value to use for the `prompt` query string parameter when making HTTP requests to the authorization endpoint. | `null` |

## Avatars as Claims

Versions of the Discord provider before version `6.0.0` would automatically map the user's avatar URL as the `urn:discord:avatar:url` claim.

This functionality is no longer built-in (see [#584](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/584) and [#585](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/pull/585)), but can be added to your application with some extra code similar to that shown in the sample below.

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddDiscord(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            options.ClaimActions.MapCustomJson("urn:discord:avatar:url", user =>
                string.Format(
                    CultureInfo.InvariantCulture,
                    "https://cdn.discordapp.com/avatars/{0}/{1}.{2}",
                    user.GetString("id"),
                    user.GetString("avatar"),
                    user.GetString("avatar").StartsWith("a_") ? "gif" : "png"));
        });
```
