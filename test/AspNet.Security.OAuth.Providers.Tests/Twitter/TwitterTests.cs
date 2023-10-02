/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Twitter;

public class TwitterTests(ITestOutputHelper outputHelper) : OAuthTests<TwitterAuthenticationOptions>(outputHelper)
{
    public override string DefaultScheme => TwitterAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddTwitter(options =>
        {
            ConfigureDefaults(builder, options);
            options.Expansions.Add("pinned_tweet_id");
            options.TweetFields.Add("text");
            options.UserFields.Add("created_at");
            options.UserFields.Add("pinned_tweet_id");
        });
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "2244994945")]
    [InlineData(ClaimTypes.Name, "TwitterDev")]
    [InlineData("urn:twitter:name", "Twitter Dev")]
    public async Task Can_Sign_In_Using_Twitter(string claimType, string claimValue)
        => await AuthenticateUserAndAssertClaimValue(claimType, claimValue);
}
