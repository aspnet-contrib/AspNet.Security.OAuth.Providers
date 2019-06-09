/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace AspNet.Security.OAuth.Apple
{
    public static class AppleClientSecretGeneratorTests
    {
        internal static readonly byte[] TestPrivateKey =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            Convert.FromBase64String("MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgU208KCg/doqiSzsVF5sknVtYSgt8/3oiYGbvryIRrzSgCgYIKoZIzj0DAQehRANCAAQfrvDWizEnWAzB2Hx2r/NyvIBO6KGBDL7wkZoKnz4Sm4+1P1dhD9fVEhbsdoq9RKEf8dvzTOZMaC/iLqZFKSN6") :
            Convert.FromBase64String("MIIEagIBAzCCBDAGCSqGSIb3DQEHAaCCBCEEggQdMIIEGTCCAw8GCSqGSIb3DQEHBqCCAwAwggL8AgEAMIIC9QYJKoZIhvcNAQcBMBwGCiqGSIb3DQEMAQYwDgQIim9BBUiXkDgCAggAgIICyGrISvsUV1o7fVvskmnfWPQqScCIDx/P93fqnes0EwtQTMIaCFIR1i9TH/6NlrusAD9qHXE7W3i/3gNsiuMGC4cYim4ym8GBhyPxWJqcubSpOVv5adja5gmz7G1v6iB6H9mg2TNLR2RQx4dddjA4Q4vSXYp2DOrvGIB8Fw9Cjx5BrQYEL78IMzB3DmNtLoFs/ny7a0WjoxnyCeT8272rzOgiuPN4Fhtj+pJYorIeoEcyJ3DAvZur6CotHANYVhSETI8tUejX5oqpg2LuY5yzxRrgy81PbabWA92TvvNMy/3/WuFSO3RyhAj4+87+ru8q6cgNT3IcZ16YXMG+XQX9eyp1EwxeDe5he3/4FEfbQH0tMD7E3MAQUjUXI/wzhCCI5HvpLXtf0N4tc8b178ykc+1oZ3zpYs4mOEHjv/xcM27C6L5YeZD36MX0LHjUQGe7ezNSYewqMR8LRwqzZ6QpLY5Dv+izx297hcKuh2bvA7MatJuI+VlK0g7gTrKYp6OHaQH6Us8F7BlO+jI8AzippV8wsJKo1VwgMVPMku4ufKAdxTuSoJP55azbze5nEebzhOidazVMgttyPrKB8QzCa19iHEzqqjWEEvDerZt5K5Em7CVxnnNVwaS/Cm70/oe4W2hpS7D4Rrj7Q4pOK6paJhxa/RZGUWtaddCSVyvI8Er4aGI0xJy1rOXBEs2yBl9z30PxsPl2ddvCV2ax0E5semTDKkFhS35OHsiFAjLUyDc0tE/Erh2+u16rzk0ceKRpmK0kanCQtqS5Amd6zaPk/D+fOIZY5tHUATHX5OGfCpT4vfTFSnJX66IAeLBOKO67sveiXrFfZanxnHAEWc6a5xHCBPZvSKlGdCsm86rZR/tXESGLWEbO3T7X7L694Rtq37yg2HjW8SQ6Y5hzYWjjLjU2F5kBJj5q9UVGIuBRUucnT+8YPJ95/00wggECBgkqhkiG9w0BBwGggfQEgfEwge4wgesGCyqGSIb3DQEMCgECoIG0MIGxMBwGCiqGSIb3DQEMAQMwDgQICc6+CvVyhsMCAggABIGQWzkumOp4ivI9Y8uECZwnmhXGn2YoZyfBjH6LC1lBtjQC6qs5PcoeqmL0Ig/6ZyPGKZ56kZXLJfWv7hnLAGcBAgHhMrLl73XNxtiIdA/FVWZvNGrzETM9U5JpfhFOZvWgoAZDeGnOirrHjBOtihaCMBscel5ZmULjJXiIr5kiVfByYX+RBrSIjaAwWRQWfK6hMSUwIwYJKoZIhvcNAQkVMRYEFF7DK7P8nOIRMzRro6ajG9hRWl04MDEwITAJBgUrDgMCGgUABBTqJGte1FPTsA+57DXU5WGnb+NfiQQIZ5eHWqzCYQwCAggA");

        [Fact]
        public static async Task GenerateAsync_Generates_Valid_Signed_Jwt()
        {
            // Arrange
            var options = new AppleAuthenticationOptions()
            {
                ClientId = "my-client-id",
                ClientSecretExpiresAfter = TimeSpan.FromMinutes(1),
                KeyId = "my-key-id",
                TeamId = "my-team-id",
                PrivateKeyBytes = (keyId) => Task.FromResult(TestPrivateKey),
            };

            await GenerateTokenAsync(options, async (generator, context) =>
            {
                var utcNow = DateTimeOffset.UtcNow;

                // Act
                string token = await generator.GenerateAsync(context);

                // Assert
                token.ShouldNotBeNullOrWhiteSpace();
                token.Count((c) => c == '.').ShouldBe(2); // Format: "{header}.{body}.{signature}"

                // Act
                var validator = new JwtSecurityTokenHandler();
                var securityToken = validator.ReadJwtToken(token);

                // Assert - See https://developer.apple.com/documentation/signinwithapplerestapi/generate_and_validate_tokens
                securityToken.ShouldNotBeNull();

                securityToken.Header.ShouldNotBeNull();
                securityToken.Header.ShouldContainKeyAndValue("alg", "ES256");
                securityToken.Header.ShouldContainKeyAndValue("kid", "my-key-id");

                securityToken.Payload.ShouldNotBeNull();
                securityToken.Payload.ShouldContainKey("exp");
                securityToken.Payload.ShouldContainKey("iat");
                securityToken.Payload.ShouldContainKeyAndValue("aud", "https://appleid.apple.com");
                securityToken.Payload.ShouldContainKeyAndValue("iss", "my-team-id");
                securityToken.Payload.ShouldContainKeyAndValue("sub", "my-client-id");

                ((long)securityToken.Payload.Iat.Value).ShouldBeGreaterThanOrEqualTo(utcNow.ToUnixTimeSeconds());
                ((long)securityToken.Payload.Exp.Value).ShouldBeGreaterThanOrEqualTo(utcNow.AddSeconds(60).ToUnixTimeSeconds());
                ((long)securityToken.Payload.Exp.Value).ShouldBeLessThanOrEqualTo(utcNow.AddSeconds(70).ToUnixTimeSeconds());
            });
        }

        [Fact]
        public static async Task GenerateAsync_Caches_Jwt_Until_Expired()
        {
            // Arrange
            var options = new AppleAuthenticationOptions()
            {
                ClientId = "my-client-id",
                ClientSecretExpiresAfter = TimeSpan.FromSeconds(1),
                KeyId = "my-key-id",
                TeamId = "my-team-id",
                PrivateKeyBytes = (keyId) => Task.FromResult(TestPrivateKey),
            };

            await GenerateTokenAsync(options, async (generator, context) =>
            {
                // Act
                string token1 = await generator.GenerateAsync(context);
                string token2 = await generator.GenerateAsync(context);

                // Assert
                token2.ShouldBe(token1);

                // Act
                await Task.Delay(TimeSpan.FromSeconds(3));
                string token3 = await generator.GenerateAsync(context);

                // Assert
                token3.ShouldNotBe(token1);
            });
        }

        private static async Task GenerateTokenAsync(
            AppleAuthenticationOptions options,
            Func<AppleClientSecretGenerator, AppleGenerateClientSecretContext, Task> actAndAssert)
        {
            // Arrange
            var builder = new WebHostBuilder()
                .Configure((app) => app.UseAuthentication())
                .ConfigureServices((services) =>
                {
                    services.AddAuthentication()
                            .AddApple();
                });

            using (var host = builder.Build())
            {
                var httpContext = new DefaultHttpContext();
                var scheme = new AuthenticationScheme("Apple", "Apple", typeof(AppleAuthenticationHandler));

                var context = new AppleGenerateClientSecretContext(httpContext, scheme, options);
                var generator = host.Services.GetRequiredService<AppleClientSecretGenerator>();

                await actAndAssert(generator, context);
            }
        }
    }
}
