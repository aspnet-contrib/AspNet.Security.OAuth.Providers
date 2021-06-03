/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Notion
{
    /// <summary>
    /// Contains constants specific to the <see cref="NotionAuthenticationHandler"/>.
    /// </summary>
    public static class NotionAuthenticationConstants
    {
        public static class Claims
        {
            public const string WorkspaceName = "urn:notion:workspace_name";
            public const string WorkspaceIcon = "urn:notion:workspace_icon";
            public const string BotId = "urn:notion:bot_id";
        }
    }
}
