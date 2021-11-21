# Integrating the Moodle Provider

Applies to the Moodle plugin [HIT-ReFreSH/moodle-local_oauth](https://github.com/HIT-ReFreSH/moodle-local_oauth).

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddMoodle(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Domain = "mymoodlesite.com";
        });
```

## Required Additional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Domain` | `string?` | The Moodle domain (_Org URL_) of your site. For example: "mymoodlesite.com". | `null` |

## Optional Settings

_None._
