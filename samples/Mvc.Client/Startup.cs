/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Mvc.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })

            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/signout";
            })

            .AddGoogle(options =>
            {
                options.ClientId = "560027070069-37ldt4kfuohhu3m495hk2j4pjp92d382.apps.googleusercontent.com";
                options.ClientSecret = "n2Q-GEw9RQjzcRbU3qhfTj8f";
            })

            .AddTwitter(options =>
            {
                options.ConsumerKey = "6XaCTaLbMqfj6ww3zvZ5g";
                options.ConsumerSecret = "Il2eFzGIrYhz6BWjYhVXBPQSfZuS4xoHpSSyD9PI";
            })

            .AddGitHub(options =>
            {
                options.ClientId = "49e302895d8b09ea5656";
                options.ClientSecret = "98f1bf028608901e9df91d64ee61536fe562064b";
                options.Scope.Add("user:email");
            })

            .AddGitLab(options =>
            {
                options.ClientId = "f4466c1688ecffbbcd9805cc0f0228c71da06d8d0554411259dfec3789711b59";
                options.ClientSecret = "f540caad65deeac7dc43f9bc7343f834888d3ce8817a2ac211c2da0133d512ba";
            })

            .AddDropbox(options =>
            {
                options.ClientId = "jpk24g2uxfxe939";
                options.ClientSecret = "qbxvkjk5la7mjp6";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
