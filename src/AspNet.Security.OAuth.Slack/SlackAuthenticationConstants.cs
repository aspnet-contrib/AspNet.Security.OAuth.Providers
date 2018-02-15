/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Slack
{
    /// <summary>
    /// Contains constants specific to the <see cref="SlackAuthenticationHandler"/>.
    /// </summary>
    public static class SlackAuthenticationConstants
    {
        public static class Claims
        {
            public const string TeamId = "urn:slack:team_id";
            public const string TeamName = "urn:slack:team_name";
            public const string UserId = "urn:slack:user_id";
        }
    }
}
