# OAuth Provider Documentation

This document contains some introductory information about how to integrate the
OAuth providers in this repository into an ASP.NET Core application.

It assumes a general familiarity with ASP.NET Core and authorization, and is
intended to demonstrate how to configure it into your application, not how it
works internally. Further integration details for a given provider can be found
in the services' own documentation, which are linked to in the main [README](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers#providers "Table of OAuth providers").

## Generic Documentation

Most of the OAuth providers in this repository can be configured by just
specifying two settings: [`ClientId`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.oauth.oauthoptions.clientid "OAuthOptions.ClientId Property") and [`ClientSecret`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.oauth.oauthoptions.clientsecret "OAuthOptions.ClientSecret Property").

Let's use the Slack provider as an example:

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddSlack(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
        });
```

With just two settings, you can configure the Slack provider to authenticate users.

The providers all follow a convention, so you just need to add the appropriate
NuGet package reference and replace the word `Slack` in the method name with the
provider name you are integrating. So for Spotify, it would be `AddSpotify()`.

## Provider-Specific Documentation

Any providers listed here have additional configuration that is either required
or optional beyond the standard built-in OAuth configuration settings.

The table below lists all of the providers that provide additional configuration
and a link to a provider-specific document for that provider. If the provider
you are using is not listed, it does not have any specific documentation and is
covered by the section above.

| Provider | Required or optional Settings | Documentation Link |
|:-:|:-:|:-:|
| Amazon | _Optional_ | [Documentation](amazon.md "Amazon provider documentation") |
| Apple | **Required** | [Documentation](sign-in-with-apple.md "Apple provider documentation") |
| BattleNet | | [Documentation](battlenet.md "BattleNet provider documentation") |
| Bitbucket | | [Documentation](bitbucket.md "Bitbucket provider documentation") |
| Discord | | [Documentation](discord.md "Discord provider documentation") |
| EVEOnline | | [Documentation](eveonline.md "EVEOnline provider documentation") |
| Foursquare | | [Documentation](foursquare.md "Foursquare provider documentation") |
| GitHub | _Optional_ | [Documentation](github.md "GitHub provider documentation") |
| Gitee | | [Documentation](gitee.md "Gitee provider documentation") |
| Instagram | | [Documentation](instagram.md "Instagram provider documentation") |
| LinkedIn | | [Documentation](linkedin.md "LinkedIn provider documentation") |
| Odnoklassniki | | [Documentation](odnoklassniki.md "Odnoklassniki provider documentation") |
| Okta | **Required** | [Documentation](okta.md "Okta provider documentation") |
| Patreon | | [Documentation](patreon.md "Patreon provider documentation") |
| QQ | | [Documentation](qq.md "QQ provider documentation") |
| Reddit | | [Documentation](reddit.md "Reddit provider documentation") |
| Salesforce | | [Documentation](salesforce.md "Salesforce provider documentation") |
| StackExchange | | [Documentation](stackexchange.md "StackExchange provider documentation") |
| SuperOffice | **Required** | [Documentation](superoffice.md "SuperOffice provider documentation") |
| Trakt | | [Documentation](trakt.md "Trakt provider documentation") |
| Twitch | | [Documentation](twitch.md "Twitch provider documentation") |
| Vkontakte | | [Documentation](vkontakte.md "Vkontakte provider documentation") |
| Weibo | | [Documentation](weibo.md "Weibo provider documentation") |
