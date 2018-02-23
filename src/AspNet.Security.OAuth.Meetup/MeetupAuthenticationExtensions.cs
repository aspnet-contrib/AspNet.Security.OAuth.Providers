/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Meetup;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Meetup authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class MeetupAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="MeetupAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Meetup authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMeetup([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddMeetup(MeetupAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="MeetupAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Meetup authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMeetup(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<MeetupAuthenticationOptions> configuration)
        {
            return builder.AddMeetup(MeetupAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="MeetupAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Meetup authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Meetup options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMeetup(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<MeetupAuthenticationOptions> configuration)
        {
            return builder.AddMeetup(scheme, MeetupAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="MeetupAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Meetup authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Meetup options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMeetup(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<MeetupAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<MeetupAuthenticationOptions, MeetupAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
