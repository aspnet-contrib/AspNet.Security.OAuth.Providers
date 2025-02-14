# Integrating the GitCode Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddGitCode(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // It appears that GitCode does not support setting the scopes through query strings.
            // The configuration of scopes (App permissions) is instead managed
            // within the OAuth app management on the official web.
        });
```

## Required Additional Settings

_None._

## Optional Settings

_None._
