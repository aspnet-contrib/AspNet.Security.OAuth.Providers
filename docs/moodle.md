# Integrating the Moodle Provider

Moodle end using [projectestac/moodle-local_oauth](https://github.com/projectestac/moodle-local_oauth).

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddMoodle(options =>
        {
            //use your own address here
            options.UseMoodleSite("https://mymoodlesite.com");

            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
        });
```

## Required Additional Settings

_None._

## Optional Settings

_None._
