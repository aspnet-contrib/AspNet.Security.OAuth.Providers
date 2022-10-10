# Sign in with Apple

The [AspNet.Security.OAuth.Apple](https://www.nuget.org/packages/AspNet.Security.OAuth.Apple/ "AspNet.Security.OAuth.Apple on NuGet.org") provider for _Sign in with Apple_ requires some custom configuration compared to the other OAuth 2.0 providers in this repository.

This document provides some additional information and context to help you configure the provider to successfully integrate _Sign in with Apple_ into your ASP.NET Core application.

## Configuration

### Client Secret

Unlike other providers, the `ClientSecret` property is not used as _Sign in with Apple_ does not use a static client secret value. Instead the client secret has to be generated using a private key file provided by Apple from the Developer Portal that is used with the Key ID and Team ID to create a signed JSON Web Token (JWT).

The provider comes with a built-in extension method `UsePrivateKey(string)` to generate they secret from a `.p8` certificate file on disk that you provide. Here's a code example:

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

Alternatively you can use the `Func<string, Task<ReadOnlyMemory<char>>> PrivateKey` property of the `AppleAuthenticationOptions` class to provide a delegate to a custom method of your own that loads the private key's bytes from another location, such as Azure Key Vault, Kubernetes secrets etc.

Below are two examples of this approach.

#### Loading from an Environment Variable

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddApple(options =>
        {
            options.ClientId = Configuration["Apple:ClientId"];
            options.KeyId = Configuration["Apple:KeyId"];
            options.TeamId = Configuration["Apple:TeamId"];
            options.PrivateKey = (keyId, _) =>
            {
                return Task.FromResult(Configuration[$"Apple:Key:{keyId}"].AsMemory());
            };
        });
```

#### Loading from Azure Key Vault

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddApple()
        .Services
        .AddOptions<AppleAuthenticationOptions>(AppleAuthenticationDefaults.AuthenticationScheme)
        .Configure<IConfiguration, SecretClient>((options, configuration, client) =>
        {
            options.ClientId = configuration["Apple:ClientId"];
            options.KeyId = configuration["Apple:KeyId"];
            options.TeamId = configuration["Apple:TeamId"];
            options.PrivateKey = async (keyId, cancellationToken) =>
            {
                var secret = await client.GetSecretAsync($"AuthKey-{keyId}", cancellationToken: cancellationToken);
                return secret.Value.Value.AsMemory();
            };
        });
```

### Issues Loading Private Key

If you encounter issues loading the private key of the certificate, the reasons could include one of the following scenarios.

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

## Sign in with Apple on iOS

When using _Sign In with Apple_ on an iOS 13+ Device, [Apple provides a different authentication workflow](https://developer.apple.com/documentation/authenticationservices) that returns the validation response to the app instead of in a server callback. Using that response to authenticate a user against your own backend requires sending the response to your servers and [communicating with the Apple authentication endpoint from there](https://developer.apple.com/documentation/sign_in_with_apple/generate_and_validate_tokens).

This workflow is out of the scope of this package but client secret generation and token validation can provide a starting point for an ASP.NET Core integration. Note that the `ClientId` in this case is the App Id where the authentication was requested, not your Services Id.

## Further Reading

Below are links to a number of other documentation sources, blog posts and sample code about Sign in with Apple.

  * [Sign In with Apple](https://developer.apple.com/sign-in-with-apple/ "Sign In with Apple - developer.apple.com")
  * [Sign In with Apple REST API](https://developer.apple.com/documentation/signinwithapplerestapi "Sign In with Apple REST API - developer.apple.com")
  * [_"What the Heck is Sign In with Apple?"_](https://developer.okta.com/blog/2019/06/04/what-the-heck-is-sign-in-with-apple "What the Heck is Sign In with Apple? - developer.okta.com")
  * [_"What is Sign In with Apple?_](https://auth0.com/blog/what-is-sign-in-with-apple-a-new-identity-provider/ "Sign In with Apple: Learn About the New Identity Provider - auth0.com")
  * [_"Prototyping Sign In with Apple for ASP.NET Core"_](https://blog.martincostello.com/sign-in-with-apple-prototype-for-aspnet-core/ "Prototyping Sign In with Apple for ASP.NET Core")
  * [Sign In with Apple demo app](https://signinwithapple.azurewebsites.net/ "Sign In with Apple demo app - signinwithapple.azurewebsites.net")

## Optional Provider Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `ClientSecretExpiresAfter` | `TimeSpan` | The period of time after which generated client secrets expire if `GenerateClientSecret` is set to `true`. | 6 months |
| `ClientSecretGenerator` | `AppleClientSecretGenerator` | A service that generates client secrets for Sign In with Apple. | _An internal implementation_ |
| `ConfigurationManager` | `IConfigurationManager<OpenIdConnectConfiguration>?` | The configuration manager to use for the OpenID configuration. | `null` |
| `GenerateClientSecret` | `bool` | Whether to automatically generate a client secret. | `false` |
| `KeyId` | `string?` | The optional ID for your Sign in with Apple private key. | `null` |
| `PrivateKeyBytes` | `Func<string, Task<ReadOnlyMemory<char>>>?` | An optional delegate to use to get the characters of the client's private key in PKCS #8 format. | `null` |
| `TeamId` | `string` | The Team ID for your Apple Developer account. | `""` |
| `TokenAudience` | `string` | The audience used for tokens. | `AppleAuthenticationConstants.Audience` |
| `TokenValidator` | `AppleIdTokenValidator` | A service that validates Apple ID tokens. | `An internal implementation` |
| `TokenValidationParameters` | `TokenValidationParameters` | The JSON Web Token validation parameters to use. | `new TokenValidationParameters()` |
| `ValidateTokens` | `bool` | Whether to validate tokens using Apple's public key. | `true` |
