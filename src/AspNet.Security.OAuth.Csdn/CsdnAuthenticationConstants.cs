namespace AspNet.Security.OAuth.Csdn
{
    /// <summary>
    /// Contains constants specific to the <see cref="CsdnAuthenticationHandler"/>.
    /// </summary>
    public static class CsdnAuthenticationConstants
    {
        public static class Claims
        {
            ///// <summary>
            ///// 当前登录用户的数字ID
            ///// </summary>
            //public const string UserId = "urn:csdn:userid";
            /// <summary>
            /// 当前登录用户的用户名，值可能为空。
            /// </summary>
            public const string UserName = "urn:csdn:username";
            /// <summary>
            /// 工作
            /// </summary>
            public const string Job = "urn:csdn:job";
            /// <summary>
            /// 工作年限
            /// </summary>
            public const string WorkYear = "urn:csdn:workyear";
            /// <summary>
            /// 网站
            /// </summary>
            public const string Website = "urn:csdn:website";
            /// <summary>
            /// 个人简介
            /// </summary>
            public const string Description = "urn:csdn:description";

        }
    }
}
