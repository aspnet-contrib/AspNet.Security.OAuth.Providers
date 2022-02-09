/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.DigitalOcean;

/// <summary>
/// Contains constants specific to the <see cref="DigitalOceanAuthenticationHandler"/>.
/// </summary>
public static class DigitalOceanAuthenticationConstants
{
    public static class Claims
    {
        /// <summary>
        /// The claim for determining if the user's email address has been verified.
        /// </summary>
        public const string EmailVerified = "urn:digitalocean:email_verified";
    }

    /// <summary>
    /// DigitalOcean API Scopes
    /// <para>https://docs.digitalocean.com/reference/api/oauth-api/</para>
    /// </summary>
    public static class Scopes
    {
        /// <summary>
        /// Read Access to the DigitalOcean account.
        /// <para>Default scope</para>
        /// </summary>
        public const string Read = "read";

        /// <summary>
        /// Write Access to the DigitalOcean account.
        /// </summary>
        public const string Write = "write";
    }

    /// <summary>
    /// DigitalOcean API Response Types
    /// <para>https://docs.digitalocean.com/reference/api/oauth-api/</para>
    /// </summary>
    public static class ResponseTypes
    {
        /// <summary>
        /// Web Application Flow.
        /// <para>Default scope</para>
        /// </summary>
        public const string Code = "code";
    }
}
