// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
// for more information concerning the license and the contributors participating to this project.

namespace AspNet.Security.OAuth.Atlassian;

public static class AtlassianOAuthenticationConstants
{
    public static class Claims
    {
        public const string AccountType = "urn:atlassian:account_type";
        public const string Picture = "urn:atlassian:picture";
        public const string AccountStatus = "urn:atlassian:account_status";
        public const string Nickname = "urn:atlassian:nickname";
        public const string ZoneInfo = "urn:atlassian:zoneinfo";
        public const string Locale = "urn:atlassian:locale";
        public const string JobTitle = "urn:atlassian:job_title";
        public const string Organization = "urn:atlassian:organization";
        public const string Department = "urn:atlassian:department";
        public const string Location = "urn:atlassian:location";
    }
}
