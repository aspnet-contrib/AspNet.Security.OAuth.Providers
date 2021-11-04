/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using static AspNet.Security.OAuth.Notion.NotionAuthenticationConstants;

namespace AspNet.Security.OAuth.Notion
{
    /// <summary>
    /// Defines a set of options used by <see cref="NotionAuthenticationHandler"/>.
    /// </summary>
    public class NotionAuthenticationOptions : OAuthOptions
    {
        public NotionAuthenticationOptions()
        {
            ClaimsIssuer = NotionAuthenticationDefaults.Issuer;

            CallbackPath = NotionAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = NotionAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = NotionAuthenticationDefaults.TokenEndpoint;

            ClaimActions.MapJsonKey(Claims.WorkspaceName, "workspace_name");
            ClaimActions.MapJsonKey(Claims.WorkspaceIcon, "workspace_icon");
            ClaimActions.MapJsonKey(Claims.BotId, "bot_id");
        }
    }
}
