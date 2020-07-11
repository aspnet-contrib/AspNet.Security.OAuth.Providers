# Integrating the LinkedIn Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddLinkedIn(options =>
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
| `EmailAddressEndpoint` | `string` | The address of the endpoint exposing the email addresses associated with the logged in user. | `BitbucketAuthenticationDefaults.EmailAddressEndpoint` |
| `Fields` | `ISet<string>` | The fields to retrieve from the user's profile. The possible values are documented [here](https://docs.microsoft.com/en-us/linkedin/consumer/integrations/self-serve/sign-in-with-linkedin#retrieving-member-profiles "Sign In with LinkedIn"). | `[ "id", "firstName", "lastName", "emailAddress" ]` |
| `MultiLocaleStringResolver` | `Func<IReadOnlyDictionary<string, string>, string?, string>` | A delegate to a method that returns a localized value for a field returned for the user's profile. | A delegate to a method that returns either the `preferredLocale`, the value for [`Thread.CurrentUICulture`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.thread.currentuiculture "Thread.CurrentUICulture Property") or the first value. |
