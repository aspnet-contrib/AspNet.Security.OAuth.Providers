/* 
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0) 
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers 
 * for more information concerning the license and the contributors participating to this project. 
 */

namespace AspNet.Security.OAuth.Alipay
{
    public static class AlipayAuthenticationConstants
    {
        public static class Claims
        {
            /// <summary>
            /// The user identifier
            /// </summary>
            public const string UserId = "urn:alipay:user_id";

            /// <summary>
            /// The avatar
            /// </summary>
            public const string Avatar = "urn:alipay:avatar";

            /// <summary>
            /// The province
            /// </summary>
            public const string Province = "urn:alipay:province";

            /// <summary>
            /// The city
            /// </summary>
            public const string City = "urn:alipay:city";

            /// <summary>
            /// The nickname
            /// </summary>
            public const string Nickname = "urn:alipay:nick_name";

            /// <summary>
            /// The is student certified. T: True; F: False;
            /// </summary>
            public const string IsStudentCertified = "urn:alipay:is_student_certified";

            /// <summary>
            /// The user type. 1: Company; 2: Person;
            /// </summary>
            public const string UserType = "urn:alipay:user_type";

            /// <summary>
            /// The user status. Q: Quick; T: Typical; B: Block; W: Waiting for Activation;
            /// </summary>
            public const string UserStatus = "urn:alipay:user_status";

            /// <summary>
            /// The is certified. T: True; F: False;
            /// </summary>
            public const string IsCertified = "urn:alipay:is_certified";

            /// <summary>
            /// The gender. F: Femal; M: Male;
            /// </summary>
            public const string Gender = "urn:alipay:gender";
        }
    }
}
