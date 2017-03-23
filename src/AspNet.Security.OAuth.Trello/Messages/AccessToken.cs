// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace AspNet.Security.OAuth.Trello
{
    /// <summary>
    /// The Trello access token retrieved from the access token endpoint.
    /// </summary>
    public class AccessToken : RequestToken
    {
        /// <summary>
        /// Gets or sets the Trello User ID.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the Trello user name.
        /// </summary>
        public string UserName { get; set; }
    }
}
