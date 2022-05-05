/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.XiaoMi;

/// <summary>
/// Contains constants specific to the <see cref="XiaoMiAuthenticationHandler"/>.
/// </summary>
public static class XiaoMiAuthenticationConstants
{
    public static class Claims
    {
        public const string MiliaoIcon = "urn:xiaomi:miliaoIcon";
        public const string MiliaoNick = "urn:xiaomi:miliaoNick";
        public const string UnionId = "urn:xiaomi:unionId";
    }
}
