// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
// for more information concerning the license and the contributors participating to this project.

namespace AspNet.Security.OAuth.Atlassian;

public static class AtlassianOAuthenticationConstants
{
    public static class Claims
    {
        public static readonly string AccountType = "urn:atlassian:account_type";
        public static readonly string Picture = "urn:atlassian:picture";
        public static readonly string AccountStatus = "urn:atlassian:account_status";
        public static readonly string Nickname = "urn:atlassian:nickname";
        public static readonly string ZoneInfo = "urn:atlassian:zoneinfo";
        public static readonly string Locale = "urn:atlassian:locale";
        public static readonly string JobTitle = "urn:atlassian:job_title";
        public static readonly string Organization = "urn:atlassian:organization";
        public static readonly string Department = "urn:atlassian:department";
        public static readonly string Location = "urn:atlassian:location";
    }
}
