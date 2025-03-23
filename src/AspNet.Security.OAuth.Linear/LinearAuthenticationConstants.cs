/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Linear;

/// <summary>
/// Contains constants specific to the <see cref="LinearAuthenticationHandler"/>.
/// </summary>
public static class LinearAuthenticationConstants
{
    public static class Claims
    {
        public const string OrganizationId = "urn:linear:organization_id";

        public const string OrganizationName = "urn:linear:organization_name";

        public const string OrganizationUrlKey = "urn:linear:organization_urlkey";
    }
}
