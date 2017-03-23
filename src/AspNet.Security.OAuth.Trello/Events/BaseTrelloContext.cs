// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Trello
{
    /// <summary>
    /// Base class for other Trello contexts.
    /// </summary>
    public class BaseTrelloContext : BaseContext
    {
        /// <summary>
        /// Initializes a <see cref="BaseTrelloContext"/>
        /// </summary>
        /// <param name="context">The HTTP environment</param>
        /// <param name="options">The options for Trello</param>
        public BaseTrelloContext(HttpContext context, TrelloAuthenticationOptions options)
            : base(context)
        {
            Options = options;
        }

        public TrelloAuthenticationOptions Options { get; }
    }
}
