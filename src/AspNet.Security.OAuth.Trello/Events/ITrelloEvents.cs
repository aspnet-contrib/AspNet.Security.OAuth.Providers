// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace AspNet.Security.OAuth.Trello
{
    /// <summary>
    /// Specifies callback methods which the <see cref="TrelloAuthenticationMiddleware"></see> invokes to enable developer control over the authentication process. />
    /// </summary>
    public interface ITrelloEvents : IRemoteAuthenticationEvents
    {
        /// <summary>
        /// Invoked whenever Trello succesfully authenticates a user
        /// </summary>
        /// <param name="context">Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.</param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        Task CreatingTicket(TrelloCreatingTicketContext context);

        /// <summary>
        /// Called when a Challenge causes a redirect to authorize endpoint in the Trello middleware
        /// </summary>
        /// <param name="context">Contains redirect URI and AuthenticationProperties of the challenge </param>
        Task RedirectToAuthorizationEndpoint(TrelloRedirectToAuthorizationEndpointContext context);
    }
}
