// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http.Authentication;

namespace AspNet.Security.OAuth.Trello
{
    /// <summary>
    /// The Trello request token obtained from the request token endpoint.
    /// </summary>
    public class RequestToken
    {
        /// <summary>
        /// Gets or sets the Trello request token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the Trello token secret.
        /// </summary>
        public string TokenSecret { get; set; }

        public bool CallbackConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a property bag for common authentication properties.
        /// </summary>
        public AuthenticationProperties Properties { get; set; }
    }
}
