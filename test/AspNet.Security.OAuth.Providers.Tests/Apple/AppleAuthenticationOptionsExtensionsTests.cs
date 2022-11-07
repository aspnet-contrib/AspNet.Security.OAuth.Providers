/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Apple;

public static class AppleAuthenticationOptionsExtensionsTests
{
    [Fact]
    public static void AddApple_Registers_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddLogging()
                .AddSingleton<IConfiguration>(configuration)
                .AddAuthentication()
                .AddApple();

        // Act
        using var serviceProvider = services.BuildServiceProvider();

        // Assert
        serviceProvider.GetRequiredService<AppleAuthenticationHandler>().ShouldNotBeNull();
        serviceProvider.GetRequiredService<IOptions<AppleAuthenticationOptions>>().ShouldNotBeNull();
        serviceProvider.GetRequiredService<IPostConfigureOptions<AppleAuthenticationOptions>>().ShouldNotBeNull();
    }
}
