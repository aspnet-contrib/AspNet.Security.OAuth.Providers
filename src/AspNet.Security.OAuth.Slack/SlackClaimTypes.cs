/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Slack
{
    /// <summary>
    /// Contains claim types specific to the <see cref="SlackAuthenticationHandler"/>.
    /// </summary>
    public static class SlackClaimTypes
    {
        public const string UserId = "urn:slack:user_id";

        public const string TeamId = "urn:slack:team_id";

        public const string TeamName = "urn:slack:team_name";
    }
}
