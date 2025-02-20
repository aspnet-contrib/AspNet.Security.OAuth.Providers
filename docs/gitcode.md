# Integrating the GitCode Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddGitCode(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // GitCode does not support setting scopes via requests to the API.
            // The configuration of scopes (App permissions) are instead managed
            // within the OAuth app management in GitCode itself.
        });
```

## Required Additional Settings

_None._

## Optional Settings

_None._
