/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using static AspNet.Security.OAuth.Alipay.AlipayAuthenticationConstants;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace AspNet.Security.OAuth.Alipay
{
    /// <summary>
    /// Defines a set of options used by <see cref="AlipayAuthenticationHandler"/>.
    /// </summary>
    public class AlipayAuthenticationOptions : OAuthOptions
    {
        public AlipayAuthenticationOptions()
        {
            ClaimsIssuer = AlipayAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(AlipayAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = AlipayAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AlipayAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AlipayAuthenticationDefaults.UserInformationEndpoint;

            GatewayUrl = AlipayAuthenticationDefaults.GatewayUrl;
            AlipayPublicKey = AlipayAuthenticationDefaults.AlipayPublicKey;
            SignType = AlipayAuthenticationDefaults.SignType;
            CharSet = AlipayAuthenticationDefaults.CharSet;
            Version = AlipayAuthenticationDefaults.Version;
            Format = AlipayAuthenticationDefaults.Format;
            IsKeyFromFile = AlipayAuthenticationDefaults.IsKeyFromFile;

            Scope.Add("auth_user");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nick_name");
            //【注意】只有is_certified为T的时候才有意义，否则不保证准确性. 性别（F：女性；M：男性）。
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender", ClaimValueTypes.Integer);

            ClaimActions.MapJsonKey(Claims.UserId, "user_id");
            ClaimActions.MapJsonKey(Claims.NickName, "nick_name");
            ClaimActions.MapJsonKey(Claims.Avatar, "avatar");
            ClaimActions.MapJsonKey(Claims.Province, "province");
            ClaimActions.MapJsonKey(Claims.City, "city");
            ClaimActions.MapJsonKey(Claims.IsStudentCertified, "is_student_certified");
            ClaimActions.MapJsonKey(Claims.UserType, "user_type");
            ClaimActions.MapJsonKey(Claims.UserStatus, "user_status");
            ClaimActions.MapJsonKey(Claims.IsCertified, "is_certified");
        }

        /// <summary>
        /// 支付宝网关
        /// </summary>
        public string GatewayUrl { get; set; } //= "https://openapi.alipay.com/gateway.do";

        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public string AlipayPublicKey { get; set; }

        /// <summary>
        /// 签名方式
        /// </summary>
        public string SignType { get; set; } //= "RSA2";

        /// <summary>
        /// 编码格式
        /// </summary>
        public string CharSet { get; set; } //= "UTF-8";

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; private set; } //= "1.0";

        /// <summary>
        /// 数据格式
        /// </summary>
        public string Format { get; private set; } //= "JSON";

        /// <summary>
        /// 是否从文件读取公私钥
        /// </summary>
        public bool IsKeyFromFile { get; set; } //= false;
    }
}