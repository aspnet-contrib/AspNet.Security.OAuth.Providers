using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Alipay.AlipayAuthenticationConstants;

namespace AspNet.Security.OAuth.Alipay
{
    /// <summary>
    /// Defines a set of options used by <see cref="AlipayAuthenticationHandler"/>.
    /// </summary>
    public class AlipayAuthenticationOptions : OAuthOptions
    {
        public string AlipayPublicKey { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [not validate sign].
        /// </summary>
        /// <value><c>true</c> if [not validate sign]; otherwise, <c>false</c>.</value>
        public bool NotValidateSign { get; set; } = false;
        public AlipayAuthenticationOptions()
        {
            ClaimsIssuer = AlipayAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(AlipayAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = AlipayAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AlipayAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AlipayAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("auth_user");

            ClaimActions.MapJsonKey(Claims.UserId, "user_id");
            ClaimActions.MapJsonKey(Claims.Avatar, "avatar");
            ClaimActions.MapJsonKey(Claims.Province, "province");
            ClaimActions.MapJsonKey(Claims.City, "city");
            ClaimActions.MapJsonKey(Claims.NickName, "nick_name");
            ClaimActions.MapJsonKey(Claims.IsStudentCertified, "is_student_certified");
            ClaimActions.MapJsonKey(Claims.UserType, "user_type");
            ClaimActions.MapJsonKey(Claims.UserStatus, "user_status");
            ClaimActions.MapJsonKey(Claims.IsCertified, "is_certified");
            ClaimActions.MapJsonKey(Claims.Gender, "gender");
        }
    }
}
