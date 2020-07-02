/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Globalization;
using System.Net.Http;
using System.Web;
using AspNet.Security.OAuth.Infrastructure;

namespace AspNet.Security.OAuth.Strava
{
    internal class StravaLoopbackRedirectHandler : LoopbackRedirectHandler
    {
        public string RequestedScope { get; set; } = "read";

        protected override Uri BuildLoopbackUri(HttpResponseMessage responseMessage)
        {
            var uri = base.BuildLoopbackUri(responseMessage);

            var builder = new UriBuilder(uri);

            var queryString = HttpUtility.ParseQueryString(builder.Query);

            queryString.Add("scope", RequestedScope);

            builder.Query = queryString.ToString();

            return builder.Uri;
        }
    }
}
