/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Gitee
{
    public class GiteeAuthenticationOptions : OAuthOptions
    {
        public string UserEmailsEndpoint { get; set; } = GiteeAuthenticationDefaults.UserEmailsEndpoint;
    }
}
