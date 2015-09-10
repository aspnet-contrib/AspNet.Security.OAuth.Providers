/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace Mvc.Client {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication();
            services.AddMvc();

            services.Configure<SharedAuthenticationOptions>(options => {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
        }

        public void Configure(IApplicationBuilder app) {
            var factory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            factory.AddConsole();

            app.UseStaticFiles();

            app.UseCookieAuthentication(options => {
                options.AutomaticAuthentication = true;
                options.AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.LoginPath = new PathString("/signin");
            });

            app.UseGoogleAuthentication(options => {
                options.ClientId = "560027070069-37ldt4kfuohhu3m495hk2j4pjp92d382.apps.googleusercontent.com";
                options.ClientSecret = "n2Q-GEw9RQjzcRbU3qhfTj8f";
            });

            app.UseTwitterAuthentication(options => {
                options.ConsumerKey = "6XaCTaLbMqfj6ww3zvZ5g";
                options.ConsumerSecret = "Il2eFzGIrYhz6BWjYhVXBPQSfZuS4xoHpSSyD9PI";
            });

            app.UseGitHubAuthentication(options => {
                options.ClientId = "49e302895d8b09ea5656";
                options.ClientSecret = "98f1bf028608901e9df91d64ee61536fe562064b";
            });

            app.UseLinkedInAuthentication(options => {
                options.ClientId = "75pgsv1r7ckn2w";
                options.ClientSecret = "C7CzxvLrYpetEmZm";
            });

            app.UseWordPressAuthentication(options => {
                options.ClientId = "42245";
                options.ClientSecret = "n18qkkc7Kpksf3EZ2GZfoRqN7jPGJkMOAcIbv3l1s0Jys7XrwhMlOlCctDmLUe0F";
            });

            app.UseYahooAuthentication(options => {
                options.ClientId = "dj0yJmk9a29HamkxMm9UT21tJmQ9WVdrOVdXdERVRE5JTnpBbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD1mZg--";
                options.ClientSecret = "bf9bacc22391a01977f8e7d57daadd564809e6a1";
            });

            // whitelisted redirect url for this id and secret: http://localhost:53507/signin-spotify
            app.UseSpotifyAuthentication(options => {
                options.ClientId = "21791719e1b14b3492c2f49607b4924c";
                options.ClientSecret = "c0141cc57a7a483c892b059b09c5ebf4";
            });

            app.UseMvc();
        }
    }
}