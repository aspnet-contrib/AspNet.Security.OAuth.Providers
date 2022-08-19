# Integrating the Snapchat Provider

## Example

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(options => /* Auth configuration */)
            .AddSnapchat(options =>
            {
                options.ClientId = "my-client-id";
                options.ClientSecret = "my-client-secret";
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
