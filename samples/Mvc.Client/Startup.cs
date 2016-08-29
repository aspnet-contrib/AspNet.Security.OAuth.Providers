/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */
 
using AspNet.Security.OAuth.GitHub;
using AspNet.Security.OAuth.VisualStudio;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Mvc.Client {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication(options => {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app) {
            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/signin"),
                LogoutPath = new PathString("/signout")
            });

            app.UseGoogleAuthentication(new GoogleOptions {
                ClientId = "560027070069-37ldt4kfuohhu3m495hk2j4pjp92d382.apps.googleusercontent.com",
                ClientSecret = "n2Q-GEw9RQjzcRbU3qhfTj8f"
            });

            app.UseTwitterAuthentication(new TwitterOptions {
                ConsumerKey = "6XaCTaLbMqfj6ww3zvZ5g",
                ConsumerSecret = "Il2eFzGIrYhz6BWjYhVXBPQSfZuS4xoHpSSyD9PI"
            });

            app.UseGitHubAuthentication(new GitHubAuthenticationOptions {
                ClientId = "49e302895d8b09ea5656",
                ClientSecret = "98f1bf028608901e9df91d64ee61536fe562064b",
                Scope = {"user:email"}
            });
            
            // You can register new app in: https://app.vsaex.visualstudio.com/app/register 
            app.UseVisualStudioAuthentication(new VisualStudioAuthenticationOptions {
                ClientId = "8DABAC41-0CAC-4BF6-BB79-670D566C55E8",
                ClientSecret = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Im9PdmN6NU1fN3AtSGpJS2xGWHo5M3VfVjBabyJ9.eyJjaWQiOiI4ZGFiYWM0MS0wY2FjLTRiZjYtYmI3OS02NzBkNTY2YzU1ZTgiLCJjc2kiOiI5NTRiZmQzNi05ZmUyLTRmMjYtOWFiYS00ZjM3MGUzOGI4MWIiLCJuYW1laWQiOiIxYTQ2NmYxYS0yZWViLTQzYWEtOTI5Yy0xMzQ5ZWFhMThjNWIiLCJpc3MiOiJhcHAudnNzcHMudmlzdWFsc3R1ZGlvLmNvbSIsImF1ZCI6ImFwcC52c3Nwcy52aXN1YWxzdHVkaW8uY29tIiwibmJmIjoxNDY2Mjg3NDY4LCJleHAiOjE2MjQwNTM4Njh9.x-waBWfWl5XqqMmvMwG7D4k0tkkfivOPlj0ZXGq__FJbU7SJ8oVBli1jJnrIX7B5D98hJe7s9FP8sUhIk4OPYOSCC5XqLYuVqr7Vo1-20uacEr9VN4nfDj5jJlw1JUtbK9g_pRLWzx9br3G1rxI4NpbWFpLtABz8wTEszC9Hlh9d1WdJK1GoyWLTu3VjTi2KIHlVqW_xv66nL9ImqX310iYT9ZOA4ljvLj_F3pK0Dc6EG6ZV5ryejvMkeA5Q8hypCwVPc2OiyIkn1CkwbjGGnZHEdRVLK2qpC2HP7FrcDj1-Tm8DlAIw5NUvvpxnmtZln5zuynPTaWyM74nsJ0b5wQ",
                SaveTokens = true
            });

            app.UseMvc();
        }
    }
}