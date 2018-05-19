AspNet.Security.OAuth.Providers
==================================

**AspNet.Security.OAuth.Providers** is a **collection of security middleware** that you can use in your **ASP.NET Core** application to support social authentication providers like **[GitHub](https://github.com/)**, **[Foursquare](https://foursquare.com/)** or **[Dropbox](https://www.dropbox.com/)**. It is directly inspired by **[Jerrie Pelser](https://github.com/jerriep)**'s initiative, **[Owin.Security.Providers](https://github.com/RockstarLabs/OwinOAuthProviders)**.

**The latest official release can be found on [NuGet](https://www.nuget.org/profiles/aspnet-contrib) and the nightly builds on [MyGet](https://www.myget.org/gallery/aspnet-contrib)**.

[![Build status](https://ci.appveyor.com/api/projects/status/3lh3pq6e57c8pnr4/branch/dev?svg=true)](https://ci.appveyor.com/project/aspnet-contrib/aspnet-security-oauth-providers/branch/dev)
[![Build status](https://travis-ci.org/aspnet-contrib/AspNet.Security.OAuth.Providers.svg?branch=dev)](https://travis-ci.org/aspnet-contrib/AspNet.Security.OAuth.Providers)

## Getting started

**Adding social authentication to your application is a breeze** and just requires a few lines in your `Startup` class:

```csharp
app.UseGitHubAuthentication(options =>
{
    options.ClientId = "49e302895d8b09ea5656";
    options.ClientSecret = "98f1bf028608901e9df91d64ee61536fe562064b";
});
```

See [https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/tree/dev/samples/Mvc.Client](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/tree/dev/samples/Mvc.Client) for a complete sample **using ASP.NET Core MVC and supporting multiple social providers**.

## Contributing

**AspNet.Security.OAuth.Providers** is actively maintained by **[Kévin Chalet](https://github.com/PinpointTownes)** ([@PinpointTownes](https://twitter.com/PinpointTownes)).

We would love it if you could help contributing to this repository.

**Special thanks to our contributors:**

* [Abhinav Nigam](https://github.com/abhinavnigam)
* [Adam Reisinger](https://github.com/Res42)
* [Albert Zakiev](https://github.com/serber)
* [Albireo](https://github.com/kappa7194)
* [Anders Blankholm](https://github.com/ablankholm)
* [Andrew Lock](https://github.com/andrewlock)
* [Andrew Mattie](https://github.com/amattie)
* [Dave Timmins](https://github.com/davetimmins)
* [Dmitry Popov](https://github.com/justdmitry)
* [Eric Green](https://github.com/ericgreenmix)
* [Ethan Celletti](https://github.com/Gekctek)
* [Igor Simovic](https://github.com/igorsimovic)
* [James Holcomb](https://github.com/jamesholcomb)
* [Jason Loeffler](https://github.com/jmloeffler)
* [Jerrie Pelser](https://github.com/jerriep)
* [Jesse Mandel](https://github.com/supergibbs)
* [Jordan Knight](https://github.com/jakkaj)
* [Kévin Chalet](https://github.com/PinpointTownes)
* [Konstantin Mamaev](https://github.com/MrMeison)
* [Mariusz Zieliński](https://github.com/mariozski)
* [Martin Costello](https://github.com/martincostello)
* [Maxime Roussin-Bélanger](https://github.com/Lorac)
* [Michael Knowles](https://github.com/mjknowles)
* [Robert Shade](https://github.com/robert-shade)
* [Sinan](https://github.com/SH2015)
* [Stefan](https://github.com/Schlurcher)
* [Steffen Wenz](https://github.com/swenz)
* [Tathagata Chakraborty](https://github.com/tatx)
* [Tommy Parnell](https://github.com/tparnell8)
* [Yannic Smeets](https://github.com/yannicsmeets)
* [zhengchun](https://github.com/zhengchun)

## Support

**Need help or wanna share your thoughts?** Don't hesitate to join us on Gitter or ask your question on StackOverflow:

- **Gitter: [https://gitter.im/aspnet-contrib/AspNet.Security.OAuth.Providers](https://gitter.im/aspnet-contrib/AspNet.Security.OAuth.Providers)**
- **StackOverflow: [https://stackoverflow.com/questions/tagged/aspnet-contrib](https://stackoverflow.com/questions/tagged/aspnet-contrib)**

## License

This project is licensed under the **Apache License**. This means that you can use, modify and distribute it freely. See [http://www.apache.org/licenses/LICENSE-2.0.html](http://www.apache.org/licenses/LICENSE-2.0.html) for more details.
