/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using static AspNet.Security.OAuth.Discord.DiscordAuthenticationConstants;

namespace AspNet.Security.OAuth.Discord
{
    public class DiscordTests : OAuthTests<DiscordAuthenticationOptions>
    {
        public DiscordTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => DiscordAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddDiscord(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "my-id")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.Email, "john@john-smith.local")]
        [InlineData(Claims.Discriminator, "1234")]
        [InlineData(Claims.AvatarHash, "dummy-avatar-hash")]
        [InlineData(Claims.AvatarUrl, "https://cdn.discordapp.com/avatars/my-id/dummy-avatar-hash.png")]
        public async Task Can_Sign_In_Using_Discord(string claimType, string claimValue)
        {
            // Arrange
            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        [Fact]
        public async Task Authorization_Endpoint_Uri_by_Default_Does_Not_Contain_Prompt()
        {
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<DiscordAuthenticationOptions>((options) =>
                {
                    options.Events = new OAuthEvents
                    {
                        OnRedirectToAuthorizationEndpoint = ctx =>
                        {
                            Assert.DoesNotContain("prompt=", ctx.RedirectUri, StringComparison.InvariantCulture);
                            ctx.Response.Redirect(ctx.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                });
            }

            using var server = CreateTestServer(ConfigureServices);
            await AuthenticateUserAsync(server);
        }

        [Fact]
        public async Task Authorization_Endpoint_Uri_Contains_Prompt_None()
        {
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<DiscordAuthenticationOptions>((options) =>
                {
                    options.DiscordAuthenticationPrompt = DiscordAuthenticationPrompt.None;
                    options.Events = new OAuthEvents
                    {
                        OnRedirectToAuthorizationEndpoint = ctx =>
                        {
                            Assert.Contains("prompt=none", ctx.RedirectUri, StringComparison.InvariantCulture);
                            ctx.Response.Redirect(ctx.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                });
            }

            using var server = CreateTestServer(ConfigureServices);
            await AuthenticateUserAsync(server);
        }

        [Fact]
        public async Task Authorization_Endpoint_Uri_Contains_Prompt_Consent()
        {
            static void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<DiscordAuthenticationOptions>((options) =>
                {
                    options.DiscordAuthenticationPrompt = DiscordAuthenticationPrompt.Consent;
                    options.Events = new OAuthEvents
                    {
                        OnRedirectToAuthorizationEndpoint = ctx =>
                        {
                            Assert.Contains("prompt=consent", ctx.RedirectUri, StringComparison.InvariantCulture);
                            ctx.Response.Redirect(ctx.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                });
            }

            using var server = CreateTestServer(ConfigureServices);
            await AuthenticateUserAsync(server);
        }
    }
}
