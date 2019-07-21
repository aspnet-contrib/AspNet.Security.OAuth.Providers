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
            Uri uri = base.BuildLoopbackUri(responseMessage);

            var builder = new UriBuilder(uri);

            var queryString = HttpUtility.ParseQueryString(builder.Query);

            queryString.Add("shop", string.Format(FormatShopParameter, ShopName));

            builder.Query = queryString.ToString();

            return builder.Uri;
        }
    }
}
