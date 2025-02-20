# OAuth Provider Documentation

This document contains some introductory information about how to integrate the
OAuth providers in this repository into an ASP.NET Core application.

It assumes a general familiarity with ASP.NET Core and authorization, and is
intended to demonstrate how to configure it for your application, not how it
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
or optional beyond the standard built-in [OAuth configuration settings](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.oauth.oauthoptions "OAuthOptions Class").

The table below lists all of the providers that provide additional configuration
and a link to a provider-specific document for that provider. If the provider
you are using is not listed, it does not have any specific documentation and is
covered by the section above.

| Provider | Required or Optional Settings | Documentation Link |
|:-:|:-:|:-:|
| Amazon | _Optional_ | [Documentation](amazon.md "Amazon provider documentation") |
| Apple | **Required** | [Documentation](sign-in-with-apple.md "Apple provider documentation") |
| BattleNet | **Required** | [Documentation](battlenet.md "BattleNet provider documentation") |
| Bitbucket | _Optional_ | [Documentation](bitbucket.md "Bitbucket provider documentation") |
| DigitalOcean | _Optional_ | [Documentation](digitalocean.md "DigitalOcean provider documentation") |
| Discord | _Optional_ | [Documentation](discord.md "Discord provider documentation") |
| Docusign | **Required** | [Documentation](docusign.md "Docusign provider documentation") |
| eBay | **Required** | [Documentation](ebay.md "eBay provider documentation") |
| EVEOnline | _Optional_ | [Documentation](eveonline.md "EVEOnline provider documentation") |
| Foursquare | _Optional_ | [Documentation](foursquare.md "Foursquare provider documentation") |
| GitCode | _Optional_ | [Documentation](gitcode.md "GitCode provider documentation") |
| GitHub | _Optional_ | [Documentation](github.md "GitHub provider documentation") |
| Gitee | _Optional_ | [Documentation](gitee.md "Gitee provider documentation") |
| Huawei | _Optional_ | [Documentation](huawei.md "Huawei provider documentation") |
| Instagram | _Optional_ | [Documentation](instagram.md "Instagram provider documentation") |
| KOOK | _Optional_ | [Documentation](kook.md "KOOK provider documentation") |
| Line | _Optional_ | [Documentation](line.md "Line provider documentation") |
| LinkedIn | _Optional_ | [Documentation](linkedin.md "LinkedIn provider documentation") |
| Odnoklassniki | _Optional_ | [Documentation](odnoklassniki.md "Odnoklassniki provider documentation") |
| Okta | **Required** | [Documentation](okta.md "Okta provider documentation") |
| Patreon | _Optional_ | [Documentation](patreon.md "Patreon provider documentation") |
| PingOne | _Optional_ | [Documentation](pingone.md "PingOne provider documentation") |
| QQ | _Optional_ | [Documentation](qq.md "QQ provider documentation") |
| Reddit | _Optional_ | [Documentation](reddit.md "Reddit provider documentation") |
| Salesforce | _Optional_ | [Documentation](salesforce.md "Salesforce provider documentation") |
| Smartsheet | _Optional_ | [Documentation](smartsheet.md "Smartsheet provider documentation") |
| Snapchat | _Optional_ | [Documentation](snapchat.md "Snapchat provider documentation") |
| StackExchange | _Optional_ | [Documentation](stackexchange.md "StackExchange provider documentation") |
| SuperOffice | **Required** | [Documentation](superoffice.md "SuperOffice provider documentation") |
| Trakt | _Optional_ | [Documentation](trakt.md "Trakt provider documentation") |
| Trovo | _Optional_ | [Documentation](trovo.md "Trovo provider documentation") |
| Twitch | _Optional_ | [Documentation](twitch.md "Twitch provider documentation") |
| Twitter | _Optional_ | [Documentation](twitter.md "Twitter provider documentation") |
| Vkontakte | _Optional_ | [Documentation](vkontakte.md "Vkontakte provider documentation") |
| Weibo | _Optional_ | [Documentation](weibo.md "Weibo provider documentation") |
| WorkWeixin (WeCom) | _Optional_ | [Documentation](workweixin.md "WorkWeixin provider documentation") |
| Xero | _Optional_ | [Documentation](xero.md "Xero provider documentation") |
| Xumm | _Optional_ | [Documentation](xumm.md "Xumm provider documentation") |
