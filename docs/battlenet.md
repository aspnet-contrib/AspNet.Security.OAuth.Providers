# Integrating the BattleNet Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddBattleNet(options =>
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Region = BattleNetAuthenticationRegion.Europe;
        });
```

## Required Additional Settings

| Property Name | Property Type | Description | Default Value |
|:--|:--|:--|:--|
| `Region` | [`BattleNetAuthenticationRegion`](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.BattleNet/BattleNetAuthenticationRegion.cs "BattleNetAuthenticationRegion enumeration") | The region used to determine the appropriate API endpoints. | `BattleNetAuthenticationRegion.America` |

## Optional Settings

_None._
