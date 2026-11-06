/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net;
using JustEat.HttpClientInterception;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace AspNet.Security.OAuth.VkId;

public class VkIdTests : OAuthTests<VkIdAuthenticationOptions>
{
    private const string TokenEndpoint = "https://id.vk.com/oauth2/auth";

    public VkIdTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
        LoopbackRedirectHandler.LoopbackParameters.Add("device_id", "1111");
        LoopbackRedirectHandler.LoopbackParameters.Add("type", "code_v2");
    }

    public override string DefaultScheme => VkIdAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddVkId(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "1234567890")]
    [InlineData(ClaimTypes.GivenName, "Ivan")]
    [InlineData(ClaimTypes.Surname, "Ivanov")]
    [InlineData(VkIdAuthenticationConstants.Claims.Avatar, "https://pp.userapi.com/60tZWMo4SmwcploUVl9XEt8ufnTTvDUmQ6Bj1g/mmv1pcj63C4.png")]
    [InlineData(ClaimTypes.Gender, "2")]
    [InlineData(VkIdAuthenticationConstants.Claims.IsVerified, "False")]
    [InlineData(ClaimTypes.DateOfBirth, "01.01.2000")]
    public async Task Can_Sign_In_Using_VkId(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);

    [Fact]
    public async Task Omits_ServiceToken_When_Not_Configured()
    {
        // Arrange
        var parameters = InterceptTokenExchange();

        using var server = CreateTestServer();

        // Act
        await AuthenticateUserAsync(server);

        // Assert
        parameters.ShouldNotBeEmpty();
        parameters.ShouldNotContainKey("service_token");
    }

    [Fact]
    public async Task Omits_ServiceToken_When_Empty()
    {
        // Arrange
        var parameters = InterceptTokenExchange();

        using var server = CreateTestServer(services =>
            services.PostConfigureAll<VkIdAuthenticationOptions>(
                options => options.ServiceToken = string.Empty));

        // Act
        await AuthenticateUserAsync(server);

        // Assert
        parameters.ShouldNotBeEmpty();
        parameters.ShouldNotContainKey("service_token");
    }

    [Fact]
    public async Task Sends_ServiceToken_When_Configured()
    {
        // Arrange
        var parameters = InterceptTokenExchange();

        using var server = CreateTestServer(services =>
            services.PostConfigureAll<VkIdAuthenticationOptions>(
                options => options.ServiceToken = "service-token"));

        // Act
        await AuthenticateUserAsync(server);

        // Assert
        parameters.ShouldContainKey("service_token");
        parameters["service_token"].Count.ShouldBe(1);
        parameters["service_token"][0].ShouldBe("service-token");
    }

    private Dictionary<string, StringValues> InterceptTokenExchange()
    {
        var parameters = new Dictionary<string, StringValues>();

        Interceptor.Deregister(HttpMethod.Post, new Uri(TokenEndpoint));

        new HttpRequestInterceptionBuilder()
            .ForPost()
            .ForUrl(TokenEndpoint)
            .Responds()
            .WithStatus(HttpStatusCode.OK)
            .WithContent(
                """
            {
              "access_token": "XXXXX",
              "refresh_token": "XXXXX",
              "id_token": "XXXXX",
              "expires_in": 0,
              "user_id": 1234567890,
              "state": "ZmFrZS1zdGF0ZS1zdHJpbmc",
              "scope": "email phone"
            }
            """)
            .WithInterceptionCallback(async request =>
            {
                var body = await request.Content!.ReadAsStringAsync();

                foreach (var parameter in QueryHelpers.ParseQuery(body))
                {
                    parameters.Add(parameter.Key, parameter.Value);
                }
            })
            .RegisterWith(Interceptor);

        return parameters;
    }
}
