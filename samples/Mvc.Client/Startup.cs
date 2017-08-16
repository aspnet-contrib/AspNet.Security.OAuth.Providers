/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.GitHub;
using AspNet.Security.OAuth.Patreon;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Mvc.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/signin"),
                LogoutPath = new PathString("/signout")
            });

            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = "560027070069-37ldt4kfuohhu3m495hk2j4pjp92d382.apps.googleusercontent.com",
                ClientSecret = "n2Q-GEw9RQjzcRbU3qhfTj8f"
            });

            app.UseTwitterAuthentication(new TwitterOptions
            {
                ConsumerKey = "6XaCTaLbMqfj6ww3zvZ5g",
                ConsumerSecret = "Il2eFzGIrYhz6BWjYhVXBPQSfZuS4xoHpSSyD9PI"
            });

            app.UseGitHubAuthentication(new GitHubAuthenticationOptions
            {
                ClientId = "0c90627999fa87bf29a5",
                ClientSecret = "52f2c61676604201f2d605111b5b63ba33a80e4b",
                Scope = { "user:email" }
            });

            app.UsePatreonAuthentication(new PatreonAuthenticationOptions()
            {
                ClientId = "cd47987eb2ccf2ecdfca1f18e692d6324c88507920af0dbc3dc176e4253b74ae",
                ClientSecret = "61145be9f187e052cf9426cc3eadc53963c3205ed7c5df72b214c860739d22a2"
            });

            app.UseMvc();
        }
    }
}