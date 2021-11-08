/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Kloudless;

/// <summary>
/// Contains constants specific to the <see cref="KloudlessAuthenticationHandler"/>.
/// </summary>
public static class KloudlessAuthenticationConstants
{
    public static class Claims
    {
        public const string Account = "urn:kloudless:account";
        public const string Created = "urn:kloudless:created";
        public const string Modified = "urn:kloudless:modified";
        public const string Service = "urn:kloudless:service";
        public const string ServiceName = "urn:kloudless:service_name";
        public const string Admin = "urn:kloudless:admin";
        public const string InternalUse = "urn:kloudless:internal_use";
        public const string Api = "urn:kloudless:api";
        public const string Apis = "urn:kloudless:apis";
        public const string EffectiveScope = "urn:kloudless:effective_scope";
        public const string Type = "urn:kloudless:type";
        public const string Enabled = "urn:kloudless:enabled";
        public const string ObjectDefinitions = "urn:kloudless:object_definitions";
        public const string CustomProperties = "urn:kloudless:custom_properties";
        public const string ProxyConnection = "urn:kloudless:proxy_connection";
        public const string Active = "urn:kloudless:active";
    }

    /// <summary>
    /// Kloudless API Scopes
    /// <para>https://developers.kloudless.com/guides/kb/scopes.html</para>
    /// </summary>
    public static class Scopes
    {
        /// <summary>
        /// Use all available services in kloudless
        /// <para>Default scope</para>
        /// </summary>
        public const string Any = "any";

        /// <summary>
        /// Cloud Storage API category
        /// </summary>
        public const string Storage = "storage";

        /// <summary>
        /// Calendar API category
        /// </summary>
        public const string Calendar = "calendar";

        /// <summary>
        /// Email API category
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// CRM API category
        /// </summary>
        public const string Crm = "crm";

        /// <summary>
        /// Messaging API category
        /// </summary>
        public const string Messaging = "messaging";

        /// <summary>
        /// ITSM API category
        /// </summary>
        public const string Itsm = "itsm";

        /// <summary>
        /// Help Desk API category
        /// </summary>
        public const string HelpDesk = "helpdesk";
    }
}
