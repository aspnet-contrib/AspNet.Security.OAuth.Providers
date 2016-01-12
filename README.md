AspNet.Security.OAuth.Providers
==================================

**AspNet.Security.OAuth.Providers** is a **collection of security middleware** that you can use in your **ASP.NET 5** application to support social authentication providers like **[GitHub](https://github.com/)**, **[Foursquare](https://foursquare.com/)** or **[Dropbox](https://www.dropbox.com/)**. It is directly inspired by **[Jerrie Pelser](https://github.com/jerriep)**'s initiative, **[Owin.Security.Providers](https://github.com/RockstarLabs/OwinOAuthProviders)**.

**The latest official release can be found on [NuGet](https://www.nuget.org/profiles/aspnet-contrib) and the nightly builds on [MyGet](https://www.myget.org/gallery/aspnet-contrib)**.

[![Build status](https://ci.appveyor.com/api/projects/status/3lh3pq6e57c8pnr4/branch/dev?svg=true)](https://ci.appveyor.com/project/aspnet-contrib/aspnet-security-oauth-providers/branch/dev)
[![Build status](https://travis-ci.org/aspnet-contrib/AspNet.Security.OAuth.Providers.svg?branch=dev)](https://travis-ci.org/aspnet-contrib/AspNet.Security.OAuth.Providers)

## Getting started

**Adding social authentication to your application is a breeze** and just requires a few lines in your `Startup` class:

    app.UseGitHubAuthentication(options => {
        options.ClientId = "49e302895d8b09ea5656";
        options.ClientSecret = "98f1bf028608901e9df91d64ee61536fe562064b";
    });

See [https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/tree/dev/samples/Mvc.Client](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/tree/dev/samples/Mvc.Client) for a complete sample **using MVC 6 and supporting multiple social providers**.

## Contributing

**AspNet.Security.OAuth.Providers** is actively maintained by **[Kévin Chalet](https://github.com/PinpointTownes)** ([@PinpointTownes](https://twitter.com/PinpointTownes)) and **[Jerrie Pelser](https://github.com/jerriep)** ([@jerriepelser](https://twitter.com/jerriepelser)).

We would love it if you could help contributing to this repository. Please look at [CONTRIBUTING.md](CONTRIBUTING.md).

**Special thanks to our contributors:**

* [Abhinav Nigam](https://github.com/abhinavnigam)
* [Adam Reisinger](https://github.com/Res42)
* [Albireo](https://github.com/kappa7194)
* [Dave Timmins](https://github.com/davetimmins)
* [Ethan Celletti](https://github.com/Gekctek)
* [Jason Loeffler](https://github.com/jmloeffler)
* [Jerrie Pelser](https://github.com/jerriep)
* [Kévin Chalet](https://github.com/PinpointTownes)
* [Michael Knowles](https://github.com/mjknowles)
* [Sinan](https://github.com/SH2015)
* [Stefan](https://github.com/Schlurcher)
* [Tathagata Chakraborty](https://github.com/tatx)
* [Tommy Parnell](https://github.com/tparnell8)
* [Yannic Smeets](https://github.com/yannicsmeets)

## Support

**Need help or wanna share your thoughts? Don't hesitate to join our dedicated chat rooms:**

- **JabbR: [https://jabbr.net/#/rooms/aspnet-contrib](https://jabbr.net/#/rooms/aspnet-contrib)**
- **Gitter: [https://gitter.im/aspnet-contrib/AspNet.Security.OAuth.Providers](https://gitter.im/aspnet-contrib/AspNet.Security.OAuth.Providers)**

## License

This project is licensed under the **Apache License**. This means that you can use, modify and distribute it freely. See [http://www.apache.org/licenses/LICENSE-2.0.html](http://www.apache.org/licenses/LICENSE-2.0.html) for more details.