/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Net;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Linq;
using AspNet.Security.OAuth.Apple;
using AspNet.Security.OAuth.Infrastructure;
using JustEat.HttpClientInterception;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace AspNet.Security.OAuth;

/// <summary>
/// The base class for integration tests for OAuth-based authentication providers.
/// </summary>
/// <typeparam name="TOptions">The options type for the authentication provider being tested.</typeparam>
public abstract class OAuthTests<TOptions> : ITestOutputHelperAccessor
    where TOptions : OAuthOptions, new()
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

                if (options is AppleAuthenticationOptions appleOptions)
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

    [Fact]
    public async Task OnCreatingTicket_Is_Raised_By_Handler_Using_Custom_Events_Type()
    {
        // Arrange
        bool onCreatingTicketEventRaised = false;

        void ConfigureServices(IServiceCollection services)
        {
            services.TryAddScoped((_) =>
            {
                return new CustomOAuthEvents()
                {
                    OnCreatingTicket = (context) =>
                    {
                        onCreatingTicketEventRaised = true;
                        return Task.CompletedTask;
                    }
                };
            });
            services.TryAddScoped((_) =>
            {
                return new CustomAppleAuthenticationEvents()
                {
                    OnCreatingTicket = (context) =>
                    {
                        onCreatingTicketEventRaised = true;
                        return Task.CompletedTask;
                    }
                };
            });

            services.PostConfigureAll<TOptions>((options) =>
            {
                if (options is AppleAuthenticationOptions appleOptions)
                {
                    appleOptions.EventsType = typeof(CustomAppleAuthenticationEvents);
                    appleOptions.TokenValidationParameters.ValidateLifetime = false;
                }
                else
                {
                    options.EventsType = typeof(CustomOAuthEvents);
                }
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        onCreatingTicketEventRaised.ShouldBeTrue();
    }

    [Fact]
    public async Task OnCreatingTicket_Is_Raised_With_Correct_User_Object_By_Handler()
    {
        // Arrange
        bool onCreatingTicketEventRaised = false;
        var userJson = new JsonElement();

        void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<TOptions>((options) =>
            {
                options.Events.OnCreatingTicket = (context) =>
                {
                    userJson = context.User;
                    onCreatingTicketEventRaised = true;
                    return Task.CompletedTask;
                };

                if (options is AppleAuthenticationOptions appleOptions)
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
        userJson.ShouldSatisfyAllConditions(
            () => userJson.ShouldNotBe(default),
            () => userJson.GetProperty("name").ShouldNotBe(default),
            () => userJson.GetProperty("name").GetString("firstName").ShouldNotBeNull(),
            () => userJson.GetProperty("name").GetString("lastName").ShouldNotBeNull());
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
                    claim.Attribute("type"!)!.Value,
                    claim.Attribute("value"!)!.Value,
                    claim.Attribute("valueType"!)!.Value,
                    claim.Attribute("issuer"!)!.Value));
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

    protected async Task<Uri> BuildChallengeUriAsync<THandler>(
        TOptions options,
        string redirectUrl,
        Func<IOptionsMonitor<TOptions>, ILoggerFactory, UrlEncoder, ISystemClock, THandler> factory,
        AuthenticationProperties? properties = null)
        where THandler : OAuthHandler<TOptions>
    {
        var scheme = new AuthenticationScheme("Test", "Test", typeof(THandler));
        var context = new DefaultHttpContext();

        properties ??= new();

        options.ClientId ??= "client-id";
        options.ClientSecret ??= "client-secret";

        if (options.Scope.Count < 1)
        {
            options.Scope.Add("scope-1");
            options.Scope.Add("scope-2");
        }

        if (options.StateDataFormat is null)
        {
            var dataProtector = new Mock<IDataProtector>();

            dataProtector.Setup((p) => p.Protect(It.IsAny<byte[]>()))
                         .Returns(Array.Empty<byte>());

            options.StateDataFormat ??= new PropertiesDataFormat(dataProtector.Object);
        }

        var mock = new Mock<IOptionsMonitor<TOptions>>();

        mock.Setup((p) => p.CurrentValue).Returns(options);
        mock.Setup((p) => p.Get(scheme.Name)).Returns(options);

        var optionsMonitor = mock.Object;
        var loggerFactory = NullLoggerFactory.Instance;
        var encoder = UrlEncoder.Default;
        var clock = new SystemClock();

        var handler = factory(optionsMonitor, loggerFactory, encoder, clock);

        await handler.InitializeAsync(scheme, context);

        var type = handler.GetType();
        var method = type.GetMethod("BuildChallengeUrl", BindingFlags.NonPublic | BindingFlags.Instance);

        object[] parameters = new object[] { properties, redirectUrl };

        string url = (string)method!.Invoke(handler, parameters)!;

        url.ShouldNotBeNullOrWhiteSpace();
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            uri.ShouldNotBeNull("The challenge URL is invalid.");
        }

        return uri;
    }

    private sealed class CustomOAuthEvents : OAuthEvents
    {
    }

    private sealed class CustomAppleAuthenticationEvents : AppleAuthenticationEvents
    {
    }
}
