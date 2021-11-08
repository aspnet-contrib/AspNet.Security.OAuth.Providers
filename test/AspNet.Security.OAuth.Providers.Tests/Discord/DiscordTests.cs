/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

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
            bool doesNotContainPrompt = false;

            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<DiscordAuthenticationOptions>((options) =>
                {
                    options.Events = new OAuthEvents
                    {
                        OnRedirectToAuthorizationEndpoint = ctx =>
                        {
                            doesNotContainPrompt = !ctx.RedirectUri.Contains("prompt=", StringComparison.InvariantCulture);
                            ctx.Response.Redirect(ctx.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                });
            }

            // Arrange
            using var server = CreateTestServer(ConfigureServices);

            // Act
            await AuthenticateUserAsync(server);

            // Assert
            doesNotContainPrompt.ShouldBeTrue();
        }

        [Fact]
        public async Task Authorization_Endpoint_Uri_Contains_Prompt_None()
        {
            bool promptIsSetToNone = false;

            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<DiscordAuthenticationOptions>((options) =>
                {
                    options.Prompt = "none";
                    options.Events = new OAuthEvents
                    {
                        OnRedirectToAuthorizationEndpoint = ctx =>
                        {
                            promptIsSetToNone = ctx.RedirectUri.Contains("prompt=none", StringComparison.InvariantCulture);
                            ctx.Response.Redirect(ctx.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                });
            }

            // Arrange
            using var server = CreateTestServer(ConfigureServices);

            // Act
            await AuthenticateUserAsync(server);

            // Assert
            promptIsSetToNone.ShouldBeTrue();
        }

        [Fact]
        public async Task Authorization_Endpoint_Uri_Contains_Prompt_Consent()
        {
            bool promptIsSetToConsent = false;

            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<DiscordAuthenticationOptions>((options) =>
                {
                    options.Prompt = "consent";
                    options.Events = new OAuthEvents
                    {
                        OnRedirectToAuthorizationEndpoint = ctx =>
                        {
                            promptIsSetToConsent = ctx.RedirectUri.Contains("prompt=consent", StringComparison.InvariantCulture);
                            ctx.Response.Redirect(ctx.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                });
            }

            // Arrange
            using var server = CreateTestServer(ConfigureServices);

            // Act
            await AuthenticateUserAsync(server);

            // Assert
            promptIsSetToConsent.ShouldBeTrue();
        }
    }
}
