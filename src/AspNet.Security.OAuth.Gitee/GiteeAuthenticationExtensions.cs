/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Gitee;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Gitee authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class GiteeAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="GiteeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Gitee authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddGitee([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddGitee(GiteeAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="GiteeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Gitee authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddGitee(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<GiteeAuthenticationOptions> configuration)
        {
            return builder.AddGitee(GiteeAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="GiteeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Gitee authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Gitee options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddGitee(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<GiteeAuthenticationOptions> configuration)
        {
            return builder.AddGitee(scheme, GiteeAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="GiteeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Gitee authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Gitee options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddGitee(
            [NotNull]this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<GiteeAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<GiteeAuthenticationOptions, GiteeAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
