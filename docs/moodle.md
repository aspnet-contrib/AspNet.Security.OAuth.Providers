# Integrating the Moodle Provider

Apply to Moodle plugin [projectestac/moodle-local_oauth](https://github.com/projectestac/moodle-local_oauth).

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddOkta(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Domain = "https://mymoodlesite.com";
        });
```

## Required Additional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Domain` | `string?` | The Moodle domain (_Org URL_) of your site. For example: "https://mymoodlesite.com". | `null` |

## Optional Settings

_None._
