using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace AspNet.Security.OAuth.Weibo
{
    /// <summary>
    /// Configuration options for <see cref="WeiboAuthenticationMiddleware"/>.
    /// </summary>
    public class WeiboAuthenticationOptions : OAuthOptions
    {
        public WeiboAuthenticationOptions()
        {
            AuthenticationScheme = WeiboAuthenticationDefaults.AuthenticationScheme;
            DisplayName = AuthenticationScheme;
            ClaimsIssuer = WeiboAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(WeiboAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = WeiboAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WeiboAuthenticationDefaults.TokenEndpoint;           
            UserInformationEndpoint = WeiboAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("email");
        }       
    }
}
