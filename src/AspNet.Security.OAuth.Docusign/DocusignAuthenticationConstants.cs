/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Docusign;

public static class DocusignAuthenticationConstants
{
    public static class Endpoints
    {
        public const string ProductionAuthorizationEndpoint = "https://account.docusign.com/oauth/auth";
        public const string SandboxAuthorizationEndpoint = "https://account-d.docusign.com/oauth/auth";

        public const string ProductionTokenEndpoint = "https://account.docusign.com/oauth/token";
        public const string SandboxTokenEndpoint = "https://account-d.docusign.com/oauth/token";

        public const string ProductionUserInformationEndpoint = "https://account.docusign.com/oauth/userinfo";
        public const string SandboxUserInformationEndpoint = "https://account-d.docusign.com/oauth/userinfo";
    }
}
