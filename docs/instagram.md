# Integrating the Instagram Provider

⚠️ The Instagram Legacy API permission was [deprecated](https://www.instagram.com/developer/ "Instagram Developer Documentation") on the 29th of June 2020. Instagram no longer supports using their Basic Display API for authentication.

From [Instagram's documentation](https://developers.facebook.com/docs/instagram-basic-display-api#authentication "Instagram Basic Display API Limitations"):

> Instagram Basic Display is not an authentication solution. Data returned by the API cannot be used to authenticate your app users or log them into your app. If you need an authentication solution we recommend using Facebook Login instead.

A Facebook authentication provider is [available from Microsoft](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins "Facebook external login setup in ASP.NET Core").

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddInstagram(options =>
        {
            options.ClientId = Configuration["Instagram:ClientId"];
            options.ClientSecret = Configuration["Instagram:ClientSecret"];

            // Optionally return the account type
            options.Fields.Add("account_type");

            // Optionally return the user's media
            options.Fields.Add("media_count");
            options.Fields.Add("media");
            options.Scope.Add("user_media");
        })
```

## Required Additional Settings

_None._

## Optional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Fields` | `ISet<string>` | The list of list of fields and edges to retrieve from the user information endpoint. | `[ "id", "username" ]` |
