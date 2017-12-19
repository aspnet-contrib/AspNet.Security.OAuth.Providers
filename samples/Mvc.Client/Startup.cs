/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
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
            
            .AddShopify(options =>
            {
                options.ApiKey = "275f6ff4f18729971335761c6a025bfe";
                options.ApiSecretKey = "35b8276265bfbda78fe8af7a89805792";

                options.Scope.Add("read_customers");
                options.Scope.Add("read_orders");
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}