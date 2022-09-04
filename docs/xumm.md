# Integrating the Xumm Provider

## Example

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(options => /* Auth configuration */)
            .AddXumm(options =>
            {
                options.ClientId = "my-api-key";
                options.ClientSecret = "my-api-secret";
            });
}
		
public void Configure(IApplicationBuilder app)
{	
    app.UseAuthentication();
    app.UseAuthorization();
}
```

## Required Additional Settings

_None._
