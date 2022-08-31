/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth.Apple;

public class AppleTests : OAuthTests<AppleAuthenticationOptions>
{
    public AppleTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => AppleAuthenticationDefaults.AuthenticationScheme;

    protected override HttpMethod RedirectMethod => HttpMethod.Post;

    protected override IDictionary<string, string> RedirectParameters { get; } = new Dictionary<string, string>()
    {
        ["user"] = @"{""name"":{""firstName"":""Johnny"",""lastName"":""Appleseed""},""email"":""johnny.appleseed@apple.local""}",
    };

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        IdentityModelEventSource.ShowPII = true;

        builder.AddApple(options =>
        {
            ConfigureDefaults(builder, options);
            options.ClientId = "com.martincostello.signinwithapple.test.client";
            options.SaveTokens = true;
        });
    }

    [Theory]
    [InlineData(ClaimTypes.Email, "johnny.appleseed@apple.local")]
    [InlineData(ClaimTypes.GivenName, "Johnny")]
    [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
    [InlineData(ClaimTypes.Surname, "Appleseed")]
    public async Task Can_Sign_In_Using_Apple_With_Client_Secret(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.GenerateClientSecret = false;
                options.ClientSecret = "my-client-secret";
                options.TokenValidationParameters.ValidateLifetime = false;
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }

    [Theory]
    [InlineData(ClaimTypes.Email, "johnny.appleseed@apple.local")]
    [InlineData(ClaimTypes.GivenName, "Johnny")]
    [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
    [InlineData(ClaimTypes.Surname, "Appleseed")]
    public async Task Can_Sign_In_Using_Apple_With_Private_Key(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.ClientSecret = string.Empty;
                options.GenerateClientSecret = true;
                options.KeyId = "my-key-id";
                options.TeamId = "my-team-id";
                options.TokenValidationParameters.ValidateLifetime = false;
                options.ValidateTokens = true;
                options.PrivateKey = async (keyId, cancellationToken) =>
                {
                    Assert.Equal("my-key-id", keyId);
                    return await TestKeys.GetPrivateKeyAsync(cancellationToken);
                };
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }

    [Theory]
    [InlineData("at_hash", "eOy0y7XVexdkzc7uuDZiCQ")]
    [InlineData("aud", "com.martincostello.signinwithapple.test.client")]
    [InlineData("auth_time", "1587211556")]
    [InlineData("email", "ussckefuz6@privaterelay.appleid.com")]
    [InlineData("email_verified", "true")]
    [InlineData("exp", "1587212159")]
    [InlineData("iat", "1587211559")]
    [InlineData("iss", "https://appleid.apple.com")]
    [InlineData("is_private_email", "true")]
    [InlineData("nonce_supported", "true")]
    [InlineData("sub", "001883.fcc77ba97500402389df96821ad9c790.1517")]
    [InlineData(ClaimTypes.Email, "ussckefuz6@privaterelay.appleid.com")]
    [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
    public async Task Can_Sign_In_Using_Apple_And_Receive_Claims_From_Id_Token(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.ClientSecret = "my-client-secret";
                options.GenerateClientSecret = false;
                options.TokenEndpoint = "https://appleid.apple.local/auth/token/email";
                options.TokenValidationParameters.ValidateLifetime = false;
                options.ValidateTokens = false;
            });
        }

        RedirectParameters.Clear(); // Simulate second sign in where user data is not returned

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }

    [Theory]
    [InlineData(ClaimTypes.Email, "johnny.appleseed@apple.local")]
    [InlineData(ClaimTypes.GivenName, "Johnny")]
    [InlineData(ClaimTypes.NameIdentifier, "001883.fcc77ba97500402389df96821ad9c790.1517")]
    [InlineData(ClaimTypes.Surname, "Appleseed")]
    public async Task Can_Sign_In_Using_Apple_With_No_Token_Validation(string claimType, string claimValue)
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.ValidateTokens = false;
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }

    [Fact]
    public async Task Cannot_Sign_In_Using_Apple_With_Expired_Token()
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.ValidateTokens = true;
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

        // Assert
        exception.InnerException.ShouldBeOfType<SecurityTokenValidationException>();
        exception.InnerException.InnerException.ShouldBeOfType<SecurityTokenExpiredException>();
    }

    [Fact]
    public async Task Cannot_Sign_In_Using_Apple_With_Invalid_Token_Audience()
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.TokenValidationParameters.ValidAudience = "my-team";
                options.TokenValidationParameters.ValidateLifetime = false;
                options.ValidateTokens = true;
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

        // Assert
        exception.InnerException.ShouldBeOfType<SecurityTokenValidationException>();
        exception.InnerException.InnerException.ShouldBeOfType<SecurityTokenInvalidAudienceException>();
    }

    [Fact]
    public async Task Cannot_Sign_In_Using_Apple_With_Invalid_Token_Issuer()
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.TokenValidationParameters.ValidIssuer = "https://apple.local";
                options.TokenValidationParameters.ValidateLifetime = false;
                options.ValidateTokens = true;
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

        // Assert
        exception.InnerException.ShouldBeOfType<SecurityTokenValidationException>();
        exception.InnerException.InnerException.ShouldBeOfType<SecurityTokenInvalidIssuerException>();
    }

    [Fact]
    public async Task Cannot_Sign_In_Using_Apple_With_Null_Token()
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.TokenEndpoint = "https://appleid.apple.local/auth/token/null";
                options.ValidateTokens = true;
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

        // Assert
        exception.InnerException.ShouldNotBeNull();
        exception.InnerException.ShouldBeOfType<InvalidOperationException>();
        exception.InnerException.Message.ShouldBe("No Apple ID token was returned in the OAuth token response.");
    }

    [Fact]
    public async Task Cannot_Sign_In_Using_Apple_With_Malformed_Token()
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.TokenEndpoint = "https://appleid.apple.local/auth/token/malformed";
                options.ValidateTokens = true;
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

        // Assert
        exception.InnerException.ShouldNotBeNull();
        exception.InnerException.ShouldBeOfType<SecurityTokenValidationException>();
        exception.InnerException.InnerException.ShouldBeOfType<ArgumentException>();
        exception.InnerException.InnerException!.Message.ShouldNotBeNull();
        exception.InnerException.InnerException.Message.ShouldStartWith("IDX");
    }

    [Fact]
    public async Task Cannot_Sign_In_Using_Apple_With_No_Token()
    {
        // Arrange
        static void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.TokenEndpoint = "https://appleid.apple.local/auth/token/none";
                options.ValidateTokens = true;
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => AuthenticateUserAsync(server));

        // Assert
        exception.InnerException.ShouldNotBeNull();
        exception.InnerException.ShouldBeOfType<InvalidOperationException>();
        exception.InnerException!.Message.ShouldNotBeNull();
        exception.InnerException.Message.ShouldBe("No Apple ID token was returned in the OAuth token response.");
    }

    [Fact]
    public async Task Custom_Events_Are_Raised_By_Handler()
    {
        // Arrange
        bool onGenerateClientSecretEventRaised = false;
        bool onValidateIdTokenEventRaised = false;

        void ConfigureServices(IServiceCollection services)
        {
            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                var onGenerateClientSecret = options.Events.OnGenerateClientSecret;

                options.Events.OnGenerateClientSecret = async (context) =>
                {
                    await onGenerateClientSecret(context);
                    onGenerateClientSecretEventRaised = true;
                };

                var onValidateIdToken = options.Events.OnValidateIdToken;

                options.Events.OnValidateIdToken = async (context) =>
                {
                    await onValidateIdToken(context);
                    onValidateIdTokenEventRaised = true;
                };

                options.ClientSecret = string.Empty;
                options.GenerateClientSecret = true;
                options.KeyId = "my-key-id";
                options.TeamId = "my-team-id";
                options.TokenValidationParameters.ValidateLifetime = false;
                options.ValidateTokens = true;
                options.PrivateKey = async (keyId, cancellationToken) =>
                {
                    Assert.Equal("my-key-id", keyId);
                    return await TestKeys.GetPrivateKeyAsync(cancellationToken);
                };
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        onGenerateClientSecretEventRaised.ShouldBeTrue();
        onValidateIdTokenEventRaised.ShouldBeTrue();
    }

    [Fact]
    public async Task Custom_Events_Are_Raised_By_Handler_Using_Custom_Events_Type()
    {
        // Arrange
        bool onGenerateClientSecretEventRaised = false;
        bool onValidateIdTokenEventRaised = false;

        void ConfigureServices(IServiceCollection services)
        {
            services.TryAddScoped((_) =>
            {
                var events = new CustomAppleAuthenticationEvents();

                var onGenerateClientSecret = events.OnGenerateClientSecret;

                events.OnGenerateClientSecret = async (context) =>
                {
                    await onGenerateClientSecret(context);
                    onGenerateClientSecretEventRaised = true;
                };

                var onValidateIdToken = events.OnValidateIdToken;

                events.OnValidateIdToken = async (context) =>
                {
                    await onValidateIdToken(context);
                    onValidateIdTokenEventRaised = true;
                };

                return events;
            });

            services.PostConfigureAll<AppleAuthenticationOptions>((options) =>
            {
                options.ClientSecret = string.Empty;
                options.EventsType = typeof(CustomAppleAuthenticationEvents);
                options.GenerateClientSecret = true;
                options.KeyId = "my-key-id";
                options.TeamId = "my-team-id";
                options.TokenValidationParameters.ValidateLifetime = false;
                options.ValidateTokens = true;
                options.PrivateKey = async (keyId, cancellationToken) =>
                {
                    Assert.Equal("my-key-id", keyId);
                    return await TestKeys.GetPrivateKeyAsync(cancellationToken);
                };
            });
        }

        using var server = CreateTestServer(ConfigureServices);

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        onGenerateClientSecretEventRaised.ShouldBeTrue();
        onValidateIdTokenEventRaised.ShouldBeTrue();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BuildChallengeUrl_Generates_Correct_Url(bool usePkce)
    {
        // Arrange
        var options = new AppleAuthenticationOptions()
        {
            UsePkce = usePkce,
        };

        string redirectUrl = "https://my-site.local/signin-zalo";

        // Act
        Uri actual = await BuildChallengeUriAsync(
            options,
            redirectUrl,
            (options, loggerFactory, encoder, clock) => new AppleAuthenticationHandler(options, loggerFactory, encoder, clock));

        // Assert
        actual.ShouldNotBeNull();
        actual.ToString().ShouldStartWith("https://appleid.apple.com/auth/authorize?");

        var query = QueryHelpers.ParseQuery(actual.Query);

        query.ShouldContainKey("state");
        query.ShouldContainKeyAndValue("client_id", options.ClientId);
        query.ShouldContainKeyAndValue("redirect_uri", redirectUrl);
        query.ShouldContainKeyAndValue("response_mode", "form_post");
        query.ShouldContainKeyAndValue("response_type", "code");
        query.ShouldContainKeyAndValue("scope", "openid name email");

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
    public void Regenerate_Test_Jwts()
    {
        using var rsa = RSA.Create();
        var parameters = rsa.ExportParameters(true);

        var webKey = new
        {
            kty = JsonWebAlgorithmsKeyTypes.RSA,
            kid = "AIDOPK1",
            use = "sig",
            alg = SecurityAlgorithms.RsaSha256,
            n = Base64UrlEncoder.Encode(parameters.Modulus),
            e = Base64UrlEncoder.Encode(parameters.Exponent),
        };

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
        };

        var audience = "com.martincostello.signinwithapple.test.client";
        var issuer = "https://appleid.apple.com";
        var expires = DateTimeOffset.FromUnixTimeSeconds(1587212159).UtcDateTime;

        var iat = new Claim(JwtRegisteredClaimNames.Iat, "1587211559");
        var sub = new Claim(JwtRegisteredClaimNames.Sub, "001883.fcc77ba97500402389df96821ad9c790.1517");
        var atHash = new Claim(JwtRegisteredClaimNames.AtHash, "eOy0y7XVexdkzc7uuDZiCQ");
        var emailVerified = new Claim("email_verified", "true");
        var authTime = new Claim(JwtRegisteredClaimNames.AuthTime, "1587211556");
        var nonceSupported = new Claim("nonce_supported", "true");

        var claimsForPublicEmail = new Claim[]
        {
            iat,
            sub,
            atHash,
            new Claim(JwtRegisteredClaimNames.Email, "johnny.appleseed@apple.local"),
            emailVerified,
            authTime,
            nonceSupported,
        };

        var publicEmailToken = new JwtSecurityToken(
            issuer,
            audience,
            claimsForPublicEmail,
            expires: expires,
            signingCredentials: signingCredentials);

        var claimsForPrivateEmail = new Claim[]
        {
            iat,
            sub,
            atHash,
            new Claim(JwtRegisteredClaimNames.Email, "ussckefuz6@privaterelay.appleid.com"),
            emailVerified,
            authTime,
            nonceSupported,
            new Claim("is_private_email", "true"),
        };

        var privateEmailToken = new JwtSecurityToken(
            issuer,
            audience,
            claimsForPrivateEmail,
            expires: expires,
            signingCredentials: signingCredentials);

        var publicEmailIdToken = new JwtSecurityTokenHandler().WriteToken(publicEmailToken);
        var privateEmailIdToken = new JwtSecurityTokenHandler().WriteToken(privateEmailToken);
        var serializedRsaPublicKey = JsonSerializer.Serialize(webKey, new JsonSerializerOptions() { WriteIndented = true });

        // Copy the values from the test output to bundles.json if you need to regenerate the JWTs to edit the claims

        // For https://appleid.apple.com/auth/keys
        OutputHelper!.WriteLine($"RSA key: {serializedRsaPublicKey}");

        // For https://appleid.apple.com/auth/token
        OutputHelper!.WriteLine($"Public email JWT: {publicEmailIdToken}");

        // For https://appleid.apple.local/auth/token/email
        OutputHelper!.WriteLine($"Private email JWT: {privateEmailIdToken}");
    }

    private sealed class CustomAppleAuthenticationEvents : AppleAuthenticationEvents
    {
    }
}
