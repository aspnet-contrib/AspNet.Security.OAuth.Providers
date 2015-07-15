AspNet.Security.OAuth.Providers
==================================

__AspNet.Security.OAuth.Providers__ is a __collection of security middleware__ that you can use in your __ASP.NET 5__ application to support social authentication providers like __[GitHub](https://github.com/)__, __[Foursquare](https://foursquare.com/)__ or __[Dropbox](https://www.dropbox.com/)__. It is directly inspired by __[Jerrie Pelser](https://github.com/jerriep)__'s initiative, __[Owin.Security.Providers](https://github.com/RockstarLabs/OwinOAuthProviders)__.

__The latest nightly builds can be found here__: __[https://www.myget.org/F/aspnet-contrib/](https://www.myget.org/F/aspnet-contrib/)__

[![Build status](https://ci.appveyor.com/api/projects/status/3lh3pq6e57c8pnr4/branch/dev?svg=true)](https://ci.appveyor.com/project/aspnet-contrib/aspnet-security-oauth-providers/branch/dev)
[![Build status](https://travis-ci.org/aspnet-contrib/AspNet.Security.OAuth.Providers.svg?branch=dev)](https://travis-ci.org/aspnet-contrib/AspNet.Security.OAuth.Providers)

## Dependencies

The __dev__ branch relies on the latest version of __DNX__ and __ASP.NET 5__, that can be found on __MyGet__: __[https://www.myget.org/gallery/aspnetvnext](https://www.myget.org/gallery/aspnetvnext)__.

Make sure to always run the latest __DNX__ version and the corresponding __ASP.NET 5__ packages by running `dnvm upgrade -u` and `dnu restore`.

## Getting started

__Adding social authentication to your application is a breeze__ and just requires a few lines in your `Startup` class:

    app.UseGitHubAuthentication(options => {
        options.ClientId = "49e302895d8b09ea5656";
        options.ClientSecret = "98f1bf028608901e9df91d64ee61536fe562064b";
    });

See [https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/tree/dev/samples/Mvc.Client](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/tree/dev/samples/Mvc.Client) for a complete sample __using MVC 6 and supporting multiple social providers__.

## Support

**Need help or wanna share your thoughts? Don't hesitate to join our dedicated chat rooms:**

- **JabbR: [https://jabbr.net/#/rooms/aspnet-contrib](https://jabbr.net/#/rooms/aspnet-contrib)**
- **Gitter: [https://gitter.im/aspnet-contrib/AspNet.Security.OAuth.Providers](https://gitter.im/aspnet-contrib/AspNet.Security.OAuth.Providers)**

## Contributors

__AspNet.Security.OAuth.Providers__ is actively maintained by __[Kévin Chalet](https://github.com/PinpointTownes)__ ([@PinpointTownes](https://twitter.com/PinpointTownes)) and __[Jerrie Pelser](https://github.com/jerriep)__ ([@jerriepelser](https://twitter.com/jerriepelser)). Contributions are welcome and can be submitted using pull requests.

## License

This project is licensed under the __Apache License__. This means that you can use, modify and distribute it freely. See [http://www.apache.org/licenses/LICENSE-2.0.html](http://www.apache.org/licenses/LICENSE-2.0.html) for more details.