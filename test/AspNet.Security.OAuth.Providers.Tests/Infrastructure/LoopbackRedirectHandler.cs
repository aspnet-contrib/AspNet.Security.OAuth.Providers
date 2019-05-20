/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.Security.OAuth.Infrastructure
{
    /// <summary>
    /// A delegating HTTP handler that loops back HTTP requests to external login providers to the local sign-in endpoint.
    /// </summary>
    internal sealed class LoopbackRedirectHandler : DelegatingHandler
    {
        public string RedirectUri { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = await base.SendAsync(request, cancellationToken);

            // Follow the redirects to external services, assuming they are OAuth-based
            if (result.StatusCode == System.Net.HttpStatusCode.Found &&
                !string.Equals(result.Headers.Location?.Host, "localhost", StringComparison.OrdinalIgnoreCase))
            {
                // Rewrite the URI to loop back to the redirected URL to simulate the user having
                // successfully authenticated with the external login page they were redirected to.
                var queryString = HttpUtility.ParseQueryString(result.Headers.Location.Query);

                string location = queryString["redirect_uri"] ?? RedirectUri;
                string state = queryString["state"];

                queryString.Clear();

                queryString.Add("code", "a6ed8e7f-471f-44f1-903b-65946475f351");
                queryString.Add("state", state);

                var builder = new UriBuilder(location)
                {
                    Query = queryString.ToString(),
                };

                var redirectRequest = new HttpRequestMessage(request.Method, builder.Uri);

                // Forward on the headers and cookies
                foreach (var header in result.Headers)
                {
                    redirectRequest.Headers.Add(header.Key, header.Value);
                }

                redirectRequest.Headers.Add("Cookie", result.Headers.GetValues("Set-Cookie"));

                // Follow the redirect URI
                return await base.SendAsync(redirectRequest, cancellationToken);
            }

            return result;
        }
    }
}
