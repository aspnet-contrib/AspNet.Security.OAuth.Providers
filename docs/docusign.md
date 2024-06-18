# Integrating the Docusign Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddDocusign(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Environment = DocusignAuthenticationEnvironment.Production;
        });
```

## Required Additional Settings

| Property Name | Property Type                                                                                                                                                                                                                             | Description                                                | Default Value                                  |
|:--|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:-----------------------------------------------------------|:-----------------------------------------------|
| `Environment` | [`DocusignAuthenticationEnvironment`](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.Docusign/DocusignAuthenticationEnvironment.cs "DocusignAuthenticationEnvironment enumeration") | The target online environment for Docusign authentication. | `DocusignAuthenticationEnvironment.Production` |

## Optional Settings

_None._
