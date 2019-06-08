/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace Mvc.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        private IConfiguration Configuration { get; }

        private IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })

            .AddCookie(options =>
            {
                options.LoginPath = "/signin";
                options.LogoutPath = "/signout";
            })

            .AddApple(options =>
            {
                options.GenerateClientSecret = true;
                options.ClientId = Configuration["AppleClientId"];
                options.KeyId = Configuration["AppleKeyId"];
                options.TeamId = Configuration["AppleTeamId"];

                options.PrivateKeyBytes = async (keyId) =>
                {
                    var privateKeyFile = HostingEnvironment.ContentRootFileProvider.GetFileInfo($"AuthKey_{keyId}.p8");
                    string privateKey;

                    using (var stream = privateKeyFile.CreateReadStream())
                    using (var reader = new StreamReader(stream))
                    {
                        privateKey = await reader.ReadToEndAsync();
                    }

                    if (privateKey.StartsWith("-----BEGIN PRIVATE KEY-----", StringComparison.Ordinal))
                    {
                        string[] keyLines = privateKey.Split('\n');
                        privateKey = string.Join(string.Empty, keyLines.Skip(1).Take(keyLines.Length - 2));
                    }
                    
                    return Convert.FromBase64String(privateKey);
                };
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

            .AddDropbox(options =>
            {
                options.ClientId = "jpk24g2uxfxe939";
                options.ClientSecret = "qbxvkjk5la7mjp6";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
