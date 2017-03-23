// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Trello
{
    /// <summary>
    /// Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.
    /// </summary>
    public class TrelloCreatingTicketContext : BaseTrelloContext
    {
        /// <summary>
        /// Initializes a <see cref="TrelloCreatingTicketContext"/>
        /// </summary>
        /// <param name="context">The HTTP environment</param>
        /// <param name="options">The options for Trello</param>
        /// <param name="userId">Trello user ID</param>
        /// <param name="userName">Trello screen name</param>
        /// <param name="accessToken">Trello access token</param>
        /// <param name="accessTokenSecret">Trello access token secret</param>
        /// <param name="user">User details</param>
        public TrelloCreatingTicketContext(
            HttpContext context,
            TrelloAuthenticationOptions options,
            string userId,
            string userName,
            string accessToken,
            string accessTokenSecret,
            JObject user)
            : base(context, options)
        {
            UserId = userId;
            UserName = userName;
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
            User = user ?? new JObject();
        }

        /// <summary>
        /// Gets the Trello user ID
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets the Trello user name
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets the Trello access token
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Gets the Trello access token secret
        /// </summary>
        public string AccessTokenSecret { get; }

        /// <summary>
        /// Gets the JSON-serialized user or an empty
        /// <see cref="JObject"/> if it is not available.
        /// </summary>
        public JObject User { get; }

        /// <summary>
        /// Gets the <see cref="ClaimsPrincipal"/> representing the user
        /// </summary>
        public ClaimsPrincipal Principal { get; set; }

        /// <summary>
        /// Gets or sets a property bag for common authentication properties
        /// </summary>
        public AuthenticationProperties Properties { get; set; }
    }
}
