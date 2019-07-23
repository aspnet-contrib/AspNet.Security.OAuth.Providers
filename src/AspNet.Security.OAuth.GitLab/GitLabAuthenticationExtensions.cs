/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.GitLab;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add GitLab authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class GitLabAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="GitLabAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables GitLab authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddGitLab([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddGitLab(GitLabAuthenticationDefaults.AuthenticationScheme, _ => { });
        }

        /// <summary>
        /// Adds <see cref="GitLabAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables GitLab authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddGitLab(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<GitLabAuthenticationOptions> configuration)
        {
            return builder.AddGitLab(GitLabAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="GitLabAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables GitLab authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the GitLab options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddGitLab(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<GitLabAuthenticationOptions> configuration)
        {
            return builder.AddGitLab(scheme, GitLabAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="GitLabAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables GitLab authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the GitLab options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddGitLab(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] string caption,
            [NotNull] Action<GitLabAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<GitLabAuthenticationOptions, GitLabAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
