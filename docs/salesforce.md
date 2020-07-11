# Integrating the Salesforce Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddSalesforce(options =>
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
| `Environment` | [`SalesforceAuthenticationEnvironment`](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.Salesforce/SalesforceAuthenticationEnvironment.cs "SalesforceAuthenticationEnvironment enumeration") | The Salesforce environment to use. | `SalesforceAuthenticationEnvironment.Production` |
