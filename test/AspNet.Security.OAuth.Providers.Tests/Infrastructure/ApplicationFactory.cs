/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AspNet.Security.OAuth.Infrastructure
{
    /// <summary>
    /// Represents a factory that creates a test application for remote OAuth-based authentication providers.
    /// </summary>
    public static class ApplicationFactory
    {
        /// <summary>
        /// Creates a test application for the specified type of authentication.
        /// </summary>
        /// <typeparam name="TOptions">The type of the configuration options for the authentication provider.</typeparam>
        /// <param name="tests">The test class to configure the application for.</param>
        /// <param name="configureServices">An optional delegate to configure additional application services.</param>
        /// <returns>
        /// The test application to use for the authentication provider.
        /// </returns>
        public static WebApplicationFactory<Program> CreateApplication<TOptions>(OAuthTests<TOptions> tests, Action<IServiceCollection>? configureServices = null)
            where TOptions : OAuthOptions
        {
#pragma warning disable CA2000
            return new TestApplicationFactory()
                .WithWebHostBuilder(builder =>
                {
                    Configure(builder, tests);

                    if (configureServices != null)
                    {
                        builder.ConfigureServices(configureServices);
                    }
                });
#pragma warning restore CA2000
        }

        private static void Configure<TOptions>(IWebHostBuilder builder, OAuthTests<TOptions> tests)
            where TOptions : OAuthOptions
        {
            // Route application logs to xunit output for debugging
            builder.ConfigureLogging(logging =>
            {
                logging.AddXUnit(tests)
                       .SetMinimumLevel(LogLevel.Information);
            });

            // Configure the test application
            builder.Configure(app => ConfigureApplication(app, tests))
                   .ConfigureServices(services =>
                    {
                        // Allow HTTP requests to external services to be intercepted
                        services.AddHttpClient();
                        services.AddSingleton<IHttpMessageHandlerBuilderFilter, HttpRequestInterceptionFilter>(
                            (_) => new HttpRequestInterceptionFilter(tests.Interceptor));

                        // Set up the test endpoint
                        services.AddRouting();

                        // Configure authentication
                        var authentication = services
                            .AddAuthentication("External")
                            .AddCookie("External", o => o.ForwardChallenge = tests.DefaultScheme);

                        tests.RegisterAuthentication(authentication);

                        services.AddAuthorization();
                    });
        }

        private static void ConfigureApplication<TOptions>(IApplicationBuilder app, OAuthTests<TOptions> tests)
            where TOptions : OAuthOptions
        {
            tests.ConfigureApplication(app);

            // Configure a single HTTP resource that challenges the client if unauthenticated
            // or returns the logged in user's claims as XML if the request is authenticated.
            app.UseRouting();

            app.UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapGet(
                       "/me",
                       async context =>
                       {
                           if (context.User.Identity?.IsAuthenticated == true)
                           {
                               string xml = IdentityToXmlString(context.User);
                               byte[] buffer = Encoding.UTF8.GetBytes(xml);

                               context.Response.StatusCode = 200;
                               context.Response.ContentType = "text/xml";

                               await context.Response.Body.WriteAsync(buffer, context.RequestAborted);
                           }
                           else
                           {
                               await tests.ChallengeAsync(context);
                           }
                       });
               });
        }

        private static string IdentityToXmlString(ClaimsPrincipal user)
        {
            var element = new XElement("claims"!);

            foreach (var identity in user.Identities)
            {
                foreach (var claim in identity.Claims)
                {
                    var node = new XElement(
                        "claim"!,
                        new XAttribute("type"!, claim.Type),
                        new XAttribute("value"!, claim.Value),
                        new XAttribute("valueType"!, claim.ValueType),
                        new XAttribute("issuer"!, claim.Issuer));

                    element.Add(node);
                }
            }

            return element.ToString();
        }

        private sealed class TestApplicationFactory : WebApplicationFactory<Program>
        {
            protected override IWebHostBuilder CreateWebHostBuilder()
            {
                return new WebHostBuilder()
                    .UseSetting("TEST_CONTENTROOT_ASPNET_SECURITY_OAUTH_PROVIDERS_TESTS", "."); // Use a dummy content root
            }
        }
    }
}
