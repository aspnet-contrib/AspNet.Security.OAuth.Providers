/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.ServiceChannel;

/// <summary>
/// Contains constants specific to the <see cref="ServiceChannelAuthenticationHandler"/>.
/// </summary>
public static class ServiceChannelAuthenticationConstants
{
    public static class Claims
    {
        public const string ProviderId = "urn:servicechannel:providerId";
        public const string ProviderName = "urn:servicechannel:providerName";
    }
}
