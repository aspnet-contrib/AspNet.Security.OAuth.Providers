AspNet.Security.OAuth.Vkontakte
==================================

## Getting started

**Adding social authentication to your application is a breeze** and just requires a few lines in your `Startup` class:

```csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddAuthentication().AddVkontakte(options =>
  {
    options.ClientId = "";
    options.ClientSecret = "";  
    options.Scope.Add("email");
  });
  
  // ...
}
```

To access the email, add `email` scope.
