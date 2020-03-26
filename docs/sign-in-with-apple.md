# Sign in with Apple

The [AspNet.Security.OAuth.Apple](https://www.nuget.org/packages/AspNet.Security.OAuth.Apple/ "AspNet.Security.OAuth.Apple on NuGet.org") provider for _Sign in with Apple_ requires some custom configuration compared to the other OAuth 2.0 providers in this repository.

This document provides some additional information and context to help you configure the provider to successfully integrate _Sign in with Apple_ into your ASP.NET Core application.

## Configuration

### Client Secret

Unlike other providers, the `ClientSecret` property is not used as _Sign in with Apple_ does not use a static client secret value. Instead the client secret has to be generated using a private key file provided by Apple from the Developer Portal that is used with the Key ID and Team ID to create a signed JSON Web Token (JWT).

The provider comes with a built-in extension method ([`UsePrivateKey(string)`](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/8e4c19008f518f3730bab90a980e01347ba6f3d3/src/AspNet.Security.OAuth.Apple/AppleAuthenticationOptionsExtensions.cs#L20-L33 "UsePrivateKey() extension method")) to generate they secret from a `.p8` certificate file on disk that you provide. Here's a [code example](https://github.com/martincostello/SignInWithAppleSample/blob/245bb70a164b66ec98ea3c2040a7387b0a3e8f0e/src/SignInWithApple/Startup.cs#L37-L46 "Example code to configure the Apple provider"):

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddApple(options =>
        {
            options.ClientId = Configuration["AppleClientId"];
            options.KeyId = Configuration["AppleKeyId"];
            options.TeamId = Configuration["AppleTeamId"];

            options.UsePrivateKey((keyId) =>
                Environment.ContentRootFileProvider.GetFileInfo($"AuthKey_{keyId}.p8"));
        });
```

Alternatively you can use the [`Func<string, Task<byte[]>> PrivateKeyBytes`](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/8e4c19008f518f3730bab90a980e01347ba6f3d3/src/AspNet.Security.OAuth.Apple/AppleAuthenticationOptions.cs#L78-L85 "Definition of PrivateKeyBytes property") property of the `AppleAuthenticationOptions` class to provide a delegate to a custom method of your own that loads the private key's bytes from another location, such as Azure Key Vault, Kubernetes secrets etc.

### Issues Loading Private Key

If you encounter issues loading the private key of the certificate, the reasons could include one of the two scenarios:

  1. Using .NET Core 2.x on Linux or macOS
  1. Using Windows Server with IIS

#### .NET Core 2.x on Linux or macOS

For the first scenario, before .NET Core 3.0 non-Windows platforms did not support loading `.p8` (PKCS #8) files. If you cannot use .NET Core 3.1 or later, it is recommended that you create a `.pfx` certificate file from your `.p8` file and use that instead.

Further information can be found in this GitHub issue: https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/390

#### Windows Server with IIS

For the second scenario, in order to load private keys Windows requires the user profile to be loaded.

This can be configured manually in IIS (or via your hosting platform's admin portal), but in some web hosting scenarios such as Azure App Service's Free and Shared tiers, it is not possible to load the user profile for security reasons due to the multi-tenant architecture of such services.

If you cannot load the user profile, possible solutions include:

  * Upgrading to a paid tier with dedicated infrastructure, such as Azure App Service's Standard tier
  * Loading the key from a `.pfx` file using the ephemeral key set ([`X509KeyStorageFlags.EphemeralKeySet`](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509keystorageflags?view=netcore-3.1 "X509KeyStorageFlags Enum on docs.microsoft.com"))

Further information can be found in this GitHub issue: https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/358

## Related Issues

Below are links to some issues raised against this repository that were related to configuration and/or environmental issues:

  * [Apple secret generation doesn't work in Azure App Service Free/Shared Tier (#358)](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/358 "Apple secret generation doesn't work in Azure App Service Free/Shared Tier")
  * [Allow passing in private key as string instead of p8 file (#385)](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/385 "Allow passing in private key as string instead of p8 file")
  * [Apple Signin redirects to a blank page 404 Error (#390)](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/390 "Apple Signin redirects to a blank page 404 Error")

## Further Reading

Below are links to a number of other documentation sources, blog posts and sample code about Sign in with Apple.

  * [Sign In with Apple](https://developer.apple.com/sign-in-with-apple/ "Sign In with Apple - developer.apple.com")
  * [Sign In with Apple REST API](https://developer.apple.com/documentation/signinwithapplerestapi "Sign In with Apple REST API - developer.apple.com")
  * [_"What the Heck is Sign In with Apple?"_](https://developer.okta.com/blog/2019/06/04/what-the-heck-is-sign-in-with-apple "What the Heck is Sign In with Apple? - developer.okta.com")
  * [_"What is Sign In with Apple?_](https://auth0.com/blog/what-is-sign-in-with-apple-a-new-identity-provider/ "Sign In with Apple: Learn About the New Identity Provider - auth0.com")
  * [_"Prototyping Sign In with Apple for ASP.NET Core"_](https://blog.martincostello.com/sign-in-with-apple-prototype-for-aspnet-core/ "Prototyping Sign In with Apple for ASP.NET Core")
  * [Sign In with Apple demo app](https://signinwithapple.azurewebsites.net/ "Sign In with Apple demo app - signinwithapple.azurewebsites.net")
