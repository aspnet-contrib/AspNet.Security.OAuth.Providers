/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Kloudless
{
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

        public static class Scopes
        {
            public const string Any = "any";
            public const string Storage = "storage";
            public const string Calendar = "calendar";
            public const string Email = "email";
            public const string CRM = "crm";
            public const string Messaging = "messaging";
            public const string ITSM = "itsm";
            public const string HelpDesk = "helpdesk";
        }
    }
}
