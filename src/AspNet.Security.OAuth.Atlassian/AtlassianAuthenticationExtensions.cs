// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
// for more information concerning the license and the contributors participating to this project.

using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Security.OAuth.Atlassian;

public static class AtlassianAuthenticationExtensions
{
    public static AuthenticationBuilder AddAtlassian(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<AtlassianAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<AtlassianAuthenticationOptions, AtlassianAuthenticationHandler>(scheme, caption, configuration);
    }
}
