using System;
using System.Net.Http;
using System.Web;
using AspNet.Security.OAuth.Infrastructure;

namespace AspNet.Security.OAuth.Shopify
{
    internal class ShopifyLoopbackRedirectHandler : LoopbackRedirectHandler
    {
        private const string FormatShopParameter = "{0}.myshopify.com";

        public ShopifyLoopbackRedirectHandler()
        {
        }

        public string ShopName { get; set; }

        protected override Uri BuildLoopbackUri(HttpResponseMessage responseMessage)
        {
            // Rewrite the URI to loop back to the redirected URL to simulate the user having
            // successfully authenticated with the external login page they were redirected to.
            var queryString = HttpUtility.ParseQueryString(responseMessage.Headers.Location.Query);

            string location = queryString["redirect_uri"] ?? RedirectUri;
            string state = queryString["state"];

            var builder = new UriBuilder(location);

            // Retain the _oauthstate parameter in redirect_uri for WeChat (see #262)
            const string OAuthStateKey = "_oauthstate";
            var redirectQuery = HttpUtility.ParseQueryString(builder.Query);
            string oauthState = redirectQuery[OAuthStateKey];

            // Remove any query string parameters we do not explictly need to retain
            queryString.Clear();

            queryString.Add("code", "a6ed8e7f-471f-44f1-903b-65946475f351");
            queryString.Add("state", state);
            queryString.Add("shop", String.Format(FormatShopParameter, ShopName));

            if (!string.IsNullOrEmpty(oauthState))
            {
                queryString.Add(OAuthStateKey, oauthState);
            }

            builder.Query = queryString.ToString();

            return builder.Uri;
        }
    }
}
