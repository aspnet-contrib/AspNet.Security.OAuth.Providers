/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using AspNet.Security.OAuth.Infrastructure;
using JustEat.HttpClientInterception;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace AspNet.Security.OAuth
{
    /// <summary>
    /// The base class for integration tests for OAuth-based authentication providers.
    /// </summary>
    /// <typeparam name="TOptions">The options type for the authentication provider being tested.</typeparam>
    public abstract class OAuthTests<TOptions> : ITestOutputHelperAccessor
        where TOptions : OAuthOptions
    {
        protected OAuthTests()
        {
            Interceptor = new HttpClientInterceptorOptions()
                .ThrowsOnMissingRegistration()
                .RegisterBundle(Path.Combine(BundleName, "bundle.json"));

            LoopbackRedirectHandler = new LoopbackRedirectHandler
            {
                RedirectMethod = RedirectMethod,
                RedirectParameters = RedirectParameters,
                RedirectUri = RedirectUri,
            };
        }

        /// <summary>
        /// Gets or sets the xunit test output helper to route application logs to.
        /// </summary>
        public ITestOutputHelper? OutputHelper { get; set; }

        /// <summary>
        /// Gets the interceptor to use for configuring stubbed HTTP responses.
        /// </summary>
        public HttpClientInterceptorOptions Interceptor { get; }

        /// <summary>
        /// Gets the name of the authentication scheme being tested.
        /// </summary>
        public abstract string DefaultScheme { get; }

        /// <summary>
        /// Gets the HTTP bundle name to use for the test.
        /// </summary>
        protected virtual string BundleName => GetType().Name.Replace("Tests", string.Empty, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the optional redirect HTTP method to use for OAuth flows.
        /// </summary>
        protected virtual HttpMethod RedirectMethod => HttpMethod.Get;

        /// <summary>
        /// Gets the optional additional parameters for the redirect request with OAuth flows.
        /// </summary>
        protected virtual IDictionary<string, string> RedirectParameters => new Dictionary<string, string>();

        /// <summary>
        /// Gets the optional redirect URI to use for OAuth flows.
        /// </summary>
        protected virtual string? RedirectUri { get; }

        /// <summary>
        /// Registers authentication for the test.
        /// </summary>
        /// <param name="builder">The authentication builder to register authentication with.</param>
        protected internal abstract void RegisterAuthentication(AuthenticationBuilder builder);

        /// <summary>
        /// Configures the test server application.
        /// Useful to add a middleware like a <see cref="Microsoft.AspNetCore.Localization.RequestLocalizationMiddleware"/> to test
        /// localization scenario.
        /// </summary>
        /// <param name="app">The application.</param>
        protected internal virtual void ConfigureApplication(IApplicationBuilder app)
        {
        }

        /// <summary>
        /// Configures the default authentication options.
        /// </summary>
        /// <param name="builder">The authentication builder to use.</param>
        /// <param name="options">The authentication options to configure.</param>
        protected virtual void ConfigureDefaults(AuthenticationBuilder builder, TOptions options)
        {
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";
            options.Backchannel = CreateBackchannel(builder);
        }

        /// <summary>
        /// Creates the test application for authentication.
        /// </summary>
        /// <param name="configureServices">An optional method to configure additional application services.</param>
        /// <returns>
        /// The test application to use for authentication.
        /// </returns>
        protected WebApplicationFactory<Program> CreateTestServer(Action<IServiceCollection>? configureServices = null)
            => ApplicationFactory.CreateApplication(this, configureServices);

        /// <summary>
        /// Creates the backchannel for an authentication provider to configures interception for HTTP requests.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>
        /// The HTTP client to use for the remote identity provider.
        /// </returns>
        protected HttpClient CreateBackchannel(AuthenticationBuilder builder)
            => builder.Services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>().CreateClient();

        public LoopbackRedirectHandler LoopbackRedirectHandler { get; set; }

        [Fact]
        public async Task OnCreatingTicket_Is_Raised_By_Handler()
        {
            // Arrange
            bool onCreatingTicketEventRaised = false;

            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<TOptions>((options) =>
                {
                    options.Events.OnCreatingTicket = (context) =>
                    {
                        onCreatingTicketEventRaised = true;
                        return Task.CompletedTask;
                    };

                    if (options is Apple.AppleAuthenticationOptions appleOptions)
                    {
                        appleOptions.TokenValidationParameters.ValidateLifetime = false;
                    }
                });
            }

            using var server = CreateTestServer(ConfigureServices);

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            onCreatingTicketEventRaised.ShouldBeTrue();
        }

        /// <summary>
        /// Run the ChannelAsync for authentication
        /// </summary>
        /// <param name="context">The HTTP context</param>
        protected internal virtual Task ChallengeAsync(HttpContext context) => context.ChallengeAsync();

        /// <summary>
        /// Asynchronously authenticates the user and returns the claims associated with the authenticated user.
        /// </summary>
        /// <param name="server">The test server to use to authenticate the user.</param>
        /// <returns>
        /// A dictionary containing the claims for the authenticated user.
        /// </returns>
        protected async Task<IDictionary<string, Claim>> AuthenticateUserAsync(WebApplicationFactory<Program> server)
        {
            IEnumerable<string> cookies;

            // Arrange - Force a request chain that challenges the request to the authentication
            // handler and returns an authentication cookie to log the user in to the application.
            using (var client = server.CreateDefaultClient(LoopbackRedirectHandler))
            {
                // Act
                using var result = await client.GetAsync("/me");

                // Assert
                result.StatusCode.ShouldBe(HttpStatusCode.Found);

                cookies = result.Headers.GetValues("Set-Cookie");
            }

            XElement element;

            // Arrange - Perform an authenticated HTTP request to get the claims for the logged-in users
            using (var client = server.CreateDefaultClient())
            {
                client.DefaultRequestHeaders.Add("Cookie", cookies);

                // Act
                using var result = await client.GetAsync("/me");

                // Assert
                result.StatusCode.ShouldBe(HttpStatusCode.OK);
                result.Content.Headers.ContentType.ShouldNotBeNull();
                result.Content.Headers.ContentType!.MediaType.ShouldBe("text/xml");

                string xml = await result.Content.ReadAsStringAsync();

                element = XElement.Parse(xml);
            }

            element.Name!.ShouldBe("claims"!);
            element.Elements("claim").Count().ShouldBeGreaterThanOrEqualTo(1);

            var claims = new List<Claim>();

            foreach (var claim in element.Elements("claim"))
            {
                claims.Add(
                    new Claim(
                        claim.Attribute("type"!) !.Value,
                        claim.Attribute("value"!) !.Value,
                        claim.Attribute("valueType"!) !.Value,
                        claim.Attribute("issuer"!) !.Value));
            }

            return claims.ToDictionary((key) => key.Type, (value) => value);
        }

        protected void AssertClaim(IDictionary<string, Claim> actual, string claimType, string claimValue)
        {
            actual.ShouldContainKey(claimType);

            string actualValue = actual[claimType].Value;

            // Parse date strings as DateTimeOffsets so that local time zone differences
            // do not cause claims which are ISO date values to fail to assert correctly.
            if (DateTimeOffset.TryParse(actualValue, out var actualAsDate) &&
                DateTimeOffset.TryParse(claimValue, out var expectedAsDate))
            {
                actualAsDate.UtcDateTime.ShouldBe(expectedAsDate.UtcDateTime);
            }
            else
            {
                actualValue.ShouldBe(claimValue);
            }
        }
    }
}
