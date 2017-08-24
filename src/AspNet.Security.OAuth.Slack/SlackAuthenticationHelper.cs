﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Slack
{
    /// <summary>
    /// Contains static methods that allow to extract user's information from a <see cref="JObject"/>
    /// instance retrieved from Slack after a successful authentication process.
    /// </summary>
    public static class SlackAuthenticationHelper
    {
        /// <summary>
        /// Gets the identifier corresponding to the authenticated user.
        /// </summary>
        public static string GetUserIdentifier([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("user_id");
        }

        /// <summary>
        /// Gets the login corresponding to the authenticated user.
        /// </summary>
        public static string GetUserName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("user");
        }

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetTeamIdentifier([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("team_id");
        }

        /// <summary>
        /// Gets the name corresponding to the authenticated user.
        /// </summary>
        public static string GetTeamName([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("team");
        }

        /// <summary>
        /// Gets the URL corresponding to the authenticated user.
        /// </summary>
        public static string GetTeamLink([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Value<string>("url");
        }

        /// <summary>
        /// Gets the identifier associated with the bot.
        /// </summary>
        public static string GetBotUserId([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["bot"]?.Value<string>("bot_user_id");
        }

        /// <summary>
        /// Gets the access token associated with the bot.
        /// </summary>
        public static string GetBotAccessToken([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["bot"]?.Value<string>("bot_access_token");
        }

        /// <summary>
        /// Gets the channel name of the selected webhook.
        /// </summary>
        public static string GetWebhookChannel([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["incoming_webhook"]?.Value<string>("channel");
        }

        /// <summary>
        /// Gets the URL of the selected webhook.
        /// </summary>
        public static string GetWebhookURL([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["incoming_webhook"]?.Value<string>("url");
        }

        /// <summary>
        /// Gets the channel configuration URL of the selected webhook.
        /// </summary>
        public static string GetWebhookConfigurationURL([NotNull] JObject user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user["incoming_webhook"]?.Value<string>("configuration_url");
        }
    }
}
