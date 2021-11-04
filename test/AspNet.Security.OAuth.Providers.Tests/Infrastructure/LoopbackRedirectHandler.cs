/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Web;

namespace AspNet.Security.OAuth.Infrastructure
{
    /// <summary>
    /// A delegating HTTP handler that loops back HTTP requests to external login providers to the local sign-in endpoint.
    /// </summary>
    public class LoopbackRedirectHandler : DelegatingHandler
    {
        public IDictionary<string, string> LoopbackParameters { get; set; } = new Dictionary<string, string>();

        public HttpMethod RedirectMethod { get; set; } = HttpMethod.Get;

        public IDictionary<string, string> RedirectParameters { get; set; } = new Dictionary<string, string>();

        public string? RedirectUri { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = await base.SendAsync(request, cancellationToken);

            // Follow the redirects to external services, assuming they are OAuth-based
            if (result.StatusCode == System.Net.HttpStatusCode.Found &&
                !string.Equals(result.Headers.Location?.Host, "localhost", StringComparison.OrdinalIgnoreCase))
            {
                var uri = BuildLoopbackUri(result);
                HttpContent? content = null;

                if (RedirectMethod == HttpMethod.Post)
                {
                    var queryString = HttpUtility.ParseQueryString(result.Headers.Location!.Query);
                    string? state = queryString["state"];

                    var parameters = new Dictionary<string, string?>()
                    {
                        ["code"] = "a6ed8e7f-471f-44f1-903b-65946475f351",
                        ["state"] = state,
                    };

                    if (RedirectParameters?.Count > 0)
                    {
                        foreach (var parameter in RedirectParameters)
                        {
                            parameters[parameter.Key] = parameter.Value;
                        }
                    }

                    content = new FormUrlEncodedContent(parameters!);
                }
                else
                {
                    uri = BuildLoopbackUri(result);
                }

                using var redirectRequest = new HttpRequestMessage(RedirectMethod, uri);

                if (content != null)
                {
                    redirectRequest.Content = content;
                }

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

        protected virtual Uri BuildLoopbackUri(HttpResponseMessage responseMessage)
        {
            // Rewrite the URI to loop back to the redirected URL to simulate the user having
            // successfully authenticated with the external login page they were redirected to.
            var queryString = HttpUtility.ParseQueryString(responseMessage.Headers.Location!.Query);

            string? location = queryString["redirect_uri"] ?? RedirectUri;
            string? state = queryString["state"];

            var builder = new UriBuilder(location!);

            // Retain the _oauthstate parameter in redirect_uri for WeChat (see #262)
            const string OAuthStateKey = "_oauthstate";
            var redirectQuery = HttpUtility.ParseQueryString(builder.Query);
            string? oauthState = redirectQuery[OAuthStateKey];

            // Remove any query string parameters we do not explictly need to retain
            queryString.Clear();

            queryString.Add("code", "a6ed8e7f-471f-44f1-903b-65946475f351");
            queryString.Add("state", state);

            if (!string.IsNullOrEmpty(oauthState))
            {
                queryString.Add(OAuthStateKey, oauthState);
            }

            if (LoopbackParameters?.Count > 0)
            {
                foreach (var parameter in LoopbackParameters)
                {
                    queryString[parameter.Key] = parameter.Value;
                }
            }

            builder.Query = queryString.ToString();

            return builder.Uri;
        }
    }
}
