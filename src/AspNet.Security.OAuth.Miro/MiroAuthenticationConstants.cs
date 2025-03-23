/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Miro;

/// <summary>
/// Contains constants specific to the <see cref="MiroAuthenticationHandler"/>.
/// </summary>
public static class MiroAuthenticationConstants
{
    public static class Claims
    {
        public const string OrganizationId = "urn:miro:organization_id";

        public const string OrganizationName = "urn:miro:organization_name";

        public const string TeamId = "urn:miro:team_id";

        public const string TeamName = "urn:miro:team_name";
    }
}
