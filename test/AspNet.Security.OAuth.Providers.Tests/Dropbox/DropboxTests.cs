/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;

namespace AspNet.Security.OAuth.Dropbox;

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
        var accessTypeIsSet = false;

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
        var refreshTokenIsPresent = false;

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

    [Theory]
    [InlineData(false, null)]
    [InlineData(true, "access-type")]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce, string? accessType)
    {
        // Arrange
        var options = new DropboxAuthenticationOptions()
        {
            AccessType = accessType,
            UsePkce = usePkce,
        };

        var redirectUrl = "https://my-site.local/signin-dropbox";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new DropboxAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://www.dropbox.com/oauth2/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "scope-1 scope-2");

        if (usePkce)
        {
            query.ShouldContainKey(OAuthConstants.CodeChallengeKey);
            query.ShouldContainKey(OAuthConstants.CodeChallengeMethodKey);
        }
        else
        {
            query.ShouldNotContainKey(OAuthConstants.CodeChallengeKey);
            query.ShouldNotContainKey(OAuthConstants.CodeChallengeMethodKey);
        }

        if (accessType is null)
        {
            query.ShouldNotContainKey("token_access_type");
        }
        else
        {
            query.ShouldContainKeyAndValue("token_access_type", accessType);
        }
    }
}
