/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Authentication;
using AspNet.Security.OAuth.Trello;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace AspNet.Security.OAuth.Trello {
    /// <summary>
    /// Defines a set of options used by <see cref="TrelloAuthenticationHandler"/>.
    /// </summary>
    public class TrelloAuthenticationOptions : RemoteAuthenticationOptions
    {
        public TrelloAuthenticationOptions() {
            AuthenticationScheme = TrelloAuthenticationDefaults.AuthenticationScheme;
            DisplayName = TrelloAuthenticationDefaults.DisplayName;
            ClaimsIssuer = TrelloAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(TrelloAuthenticationDefaults.CallbackPath);

            RequestTokenEndpoint = TrelloAuthenticationDefaults.RequestTokenEndpoint;
            AuthorizeTokenEndpoint = TrelloAuthenticationDefaults.AuthorizeTokenEndpoint;
            AccessTokenEndpoint = TrelloAuthenticationDefaults.AccessTokenEndpoint;

            BackchannelTimeout = TimeSpan.FromSeconds(60);
            Events = new TrelloEvents();
        }

        public string AccessTokenEndpoint { get; set; }
        public string AuthorizeTokenEndpoint { get; set; }
        public string RequestTokenEndpoint { get; set; }
        public string AppName { get; set; }

        /// <summary>
        /// Gets or sets the consumer key used to communicate with Trello.
        /// </summary>
        /// <value>The consumer key used to communicate with Trello.</value>
        public string ConsumerKey { get; set; }

        /// <summary>
        /// Gets or sets the consumer secret used to sign requests to Trello.
        /// </summary>
        /// <value>The consumer secret used to sign requests to Trello.</value>
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// Enables the retrieval user details during the authentication process.
        /// </summary>
        public bool RetrieveUserDetails { get; set; }

        /// <summary>
        /// Gets or sets the type used to secure data handled by the middleware.
        /// </summary>
        public ISecureDataFormat<RequestToken> StateDataFormat { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ITrelloEvents"/> used to handle authentication events.
        /// </summary>
        public new ITrelloEvents Events
        {
            get { return (ITrelloEvents)base.Events; }
            set { base.Events = value; }
        }

        /// <summary>
        /// For testing purposes only.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ISystemClock SystemClock { get; set; } = new SystemClock();
    }
}
