/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Dropbox
{
    public class DropboxTests : OAuthTests<DropboxAuthenticationOptions>
    {
        public DropboxTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => DropboxAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddDropbox(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.NameIdentifier, "dbid:AAH4f99T0taONIb-OurWxbNQ6ywGRopQngc")]
        [InlineData(ClaimTypes.Name, "Franz Ferdinand (Personal)")]
        [InlineData(ClaimTypes.Email, "franz@gmail.com")]
        public async Task Can_Sign_In_Using_Dropbox(string claimType, string claimValue)
        {
            // Arrange
            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }

        [Theory]
        [InlineData("offline")]
        [InlineData("online")]
        [InlineData("legacy")]
        public async Task RedirectUri_Contains_Access_Type(string value)
        {
            bool accessTypeIsSet = false;

            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<DropboxAuthenticationOptions>((options) =>
                {
                    options.AccessType = value;
                    options.Events = new OAuthEvents
                    {
                        OnRedirectToAuthorizationEndpoint = ctx =>
                        {
                            accessTypeIsSet = ctx.RedirectUri.Contains($"token_access_type={value}", StringComparison.OrdinalIgnoreCase);
                            ctx.Response.Redirect(ctx.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                });
            }

            // Arrange
            using var server = CreateTestServer(ConfigureServices);

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            accessTypeIsSet.ShouldBeTrue();
        }

        [Fact]
        public async Task Response_Contains_Refresh_Token()
        {
            bool refreshTokenIsPresent = false;

            void ConfigureServices(IServiceCollection services)
            {
                services.PostConfigureAll<DropboxAuthenticationOptions>((options) =>
                {
                    options.AccessType = "offline";
                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = ctx =>
                        {
                            refreshTokenIsPresent = !string.IsNullOrEmpty(ctx.RefreshToken);
                            return Task.CompletedTask;
                        }
                    };
                });
            }

            // Arrange
            using var server = CreateTestServer(ConfigureServices);

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            refreshTokenIsPresent.ShouldBeTrue();
        }
    }
}
