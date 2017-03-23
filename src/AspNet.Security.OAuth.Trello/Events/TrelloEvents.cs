// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;


namespace AspNet.Security.OAuth.Trello
{
    /// <summary>
    /// Default <see cref="ITrelloEvents"/> implementation.
    /// </summary>
    public class TrelloEvents : RemoteAuthenticationEvents, ITrelloEvents
    {
        /// <summary>
        /// Gets or sets the function that is invoked when the Authenticated method is invoked.
        /// </summary>
        public Func<TrelloCreatingTicketContext, Task> OnCreatingTicket { get; set; } = context => TaskCache.CompletedTask;

        /// <summary>
        /// Gets or sets the delegate that is invoked when the ApplyRedirect method is invoked.
        /// </summary>
        public Func<TrelloRedirectToAuthorizationEndpointContext, Task> OnRedirectToAuthorizationEndpoint { get; set; } = context =>
        {
            context.Response.Redirect(context.RedirectUri);
            return TaskCache.CompletedTask;
        };

        /// <summary>
        /// Invoked whenever Trello successfully authenticates a user
        /// </summary>
        /// <param name="context">Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.</param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        public virtual Task CreatingTicket(TrelloCreatingTicketContext context) => OnCreatingTicket(context);

        /// <summary>
        /// Called when a Challenge causes a redirect to authorize endpoint in the Trello middleware
        /// </summary>
        /// <param name="context">Contains redirect URI and AuthenticationProperties of the challenge </param>
        public virtual Task RedirectToAuthorizationEndpoint(TrelloRedirectToAuthorizationEndpointContext context) => OnRedirectToAuthorizationEndpoint(context);
    }
}
