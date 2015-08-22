using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.LinkedIn {
    /// <summary>
    /// Defines a set of options used by <see cref="LinkedInAuthenticationHandler"/>.
    /// </summary>
    public class LinkedInAuthenticationOptions : OAuthAuthenticationOptions
    {
        public LinkedInAuthenticationOptions() {
            AuthenticationScheme = LinkedInAuthenticationDefaults.AuthenticationScheme;
            Caption = LinkedInAuthenticationDefaults.Caption;
            ClaimsIssuer = LinkedInAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(LinkedInAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = LinkedInAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = LinkedInAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = LinkedInAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;

        }
    }
}