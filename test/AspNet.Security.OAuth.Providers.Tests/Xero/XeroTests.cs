/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Xero;

public class XeroTests : OAuthTests<XeroAuthenticationOptions>
{
    public XeroTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => XeroAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

        builder.AddXero(options =>
        {
            ConfigureDefaults(builder, options);

            options.ClientId = "76645D17ED29411487E0D9E5DE2B493B";
            options.SaveTokens = true;
            options.TokenValidationParameters.ValidAudience = options.ClientId;
            options.TokenValidationParameters.ValidIssuer = "https://identity.xero.com";

            options.TokenValidationParameters.ValidateLifetime = false;
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "8ba85fb8-f540-4eb1-b53d-0519da53c47b")]
    [InlineData(ClaimTypes.Email, "fecopij273@icesilo.com")]
    [InlineData("iss", "https://identity.xero.com")]
    [InlineData("aud", "76645D17ED29411487E0D9E5DE2B493B")]
    [InlineData("sid", "c37c8dce142c4ce98c1d646df83dafeb")]
    [InlineData("global_session_id", "c37c8dce142c4ce98c1d646df83dafeb")]
    public async Task Can_Sign_In_Using_Xero(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce)
    {
        // Arrange
        var options = new XeroAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        var redirectUrl = "https://my-site.local/signin-xero";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder) => new XeroAuthenticationHandler(options, loggerFactory, encoder));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://login.xero.com/identity/connect/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "openid");

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
    }

    [Fact]
    public async Task Cannot_Sign_In_Using_Xero_With_Invalid_Token_Audience()
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<XeroAuthenticationOptions>((options) =>
            {
                options.TokenValidationParameters.ValidAudience = "not-the-right-audience";
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var exception = await Assert.ThrowsAsync<AuthenticationFailureException>(() => AuthenticateUserAsync(server));

        // Assert
        exception.InnerException.ShouldBeOfType<SecurityTokenValidationException>();
    }
}
