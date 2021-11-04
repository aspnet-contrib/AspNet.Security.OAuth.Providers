/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Coinbase
{
    public class CoinbaseTests : OAuthTests<CoinbaseAuthenticationOptions>
    {
        public CoinbaseTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        public override string DefaultScheme => CoinbaseAuthenticationDefaults.AuthenticationScheme;

        protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
        {
            builder.AddCoinbase(options => ConfigureDefaults(builder, options));
        }

        [Theory]
        [InlineData(ClaimTypes.Email, "john.smith@coinbase.local")]
        [InlineData(ClaimTypes.Name, "John Smith")]
        [InlineData(ClaimTypes.NameIdentifier, "9da7a204-544e-5fd1-9a12-61176c5d4cd8")]
        [InlineData("urn:coinbase:avatar_url", "https://images.coinbase.com/avatar?h=vR%2FY8igBoPwuwGren5JMwvDNGpURAY%2F0nRIOgH%2FY2Qh%2BQ6nomR3qusA%2Bh6o2%0Af9rH&s=128")]
        [InlineData("urn:coinbase:profile_bio", "test")]
        [InlineData("urn:coinbase:profile_location", "test")]
        [InlineData("urn:coinbase:profile_url", "https://coinbase.com/jsmith")]
        [InlineData("urn:coinbase:username", "jsmith")]
        public async Task Can_Sign_In_Using_Coinbase(string claimType, string claimValue)
        {
            // Arrange
            using var server = CreateTestServer();

            // Act
            var claims = await AuthenticateUserAsync(server);

            // Assert
            AssertClaim(claims, claimType, claimValue);
        }
    }
}
