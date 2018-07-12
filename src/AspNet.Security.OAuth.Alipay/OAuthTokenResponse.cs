using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNet.Security.OAuth.Alipay
{
    public class AlipayOAuthTokenResponse : OAuthTokenResponse
    {
        //private AlipayOAuthTokenResponse(JObject response)
        //{
        //    UserId = response.Value<string>("user_id");
        //    ReExpiresIn = response.Value<string>("re_expires_in");
        //}

        /// <summary>
        /// 支付宝用户的唯一userId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 刷新令牌的有效时间，单位是秒。
        /// </summary>
        public string ReExpiresIn { get; set; }
    }
}
