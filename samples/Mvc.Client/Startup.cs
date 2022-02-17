/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Logging;

namespace Mvc.Client;

public class Startup
{
    public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
    {
        Configuration = configuration;
        HostingEnvironment = hostingEnvironment;
    }

    public IConfiguration Configuration { get; }

    private IHostEnvironment HostingEnvironment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })

        .AddCookie(options =>
        {
            options.LoginPath = "/signin";
            options.LogoutPath = "/signout";
        })

        .AddGoogle(options =>
        {
            options.ClientId = Configuration["Google:ClientId"] ?? string.Empty;
            options.ClientSecret = Configuration["Google:ClientSecret"] ?? string.Empty;
        })

        .AddTwitter(options =>
        {
            options.ClientId = Configuration["Twitter:ClientId"] ?? string.Empty;
            options.ClientSecret = Configuration["Twitter:ClientSecret"] ?? string.Empty;
        })

        .AddGitHub(options =>
        {
            options.ClientId = Configuration["GitHub:ClientId"] ?? string.Empty;
            options.ClientSecret = Configuration["GitHub:ClientSecret"] ?? string.Empty;
            options.EnterpriseDomain = Configuration["GitHub:EnterpriseDomain"] ?? string.Empty;
            options.Scope.Add("user:email");
        })

        /*
        .AddApple(options =>
        {
            options.ClientId = Configuration["Apple:ClientId"] ?? string.Empty;
            options.KeyId = Configuration["Apple:KeyId"] ?? string.Empty;
            options.TeamId = Configuration["Apple:TeamId"] ?? string.Empty;
            options.UsePrivateKey(
                (keyId) => HostingEnvironment.ContentRootFileProvider.GetFileInfo($"AuthKey_{keyId}.p8"));
        })
        */

        .AddDropbox(options =>
        {
            options.ClientId = Configuration["Dropbox:ClientId"] ?? string.Empty;
            options.ClientSecret = Configuration["Dropbox:ClientSecret"] ?? string.Empty;
        });

        services.AddMvc();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (HostingEnvironment.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
        }

        // Required to serve files with no extension in the .well-known folder
        var options = new StaticFileOptions()
        {
            ServeUnknownFileTypes = true,
        };

        app.UseStaticFiles(options);

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}
