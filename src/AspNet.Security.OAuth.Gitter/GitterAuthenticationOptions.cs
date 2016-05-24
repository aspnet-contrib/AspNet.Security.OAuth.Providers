using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Gitter
{
    public class GitterAuthenticationOptions : OAuthOptions
    {
        public GitterAuthenticationOptions()
        {
            AuthenticationScheme = GitterAuthenticationDefaults.AuthenticationScheme;
            DisplayName = GitterAuthenticationDefaults.DisplayName;
            ClaimsIssuer = GitterAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(GitterAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = GitterAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = GitterAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = GitterAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}