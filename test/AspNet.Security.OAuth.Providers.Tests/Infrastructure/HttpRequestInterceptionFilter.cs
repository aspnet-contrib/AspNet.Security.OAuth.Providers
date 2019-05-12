/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using JustEat.HttpClientInterception;
using Microsoft.Extensions.Http;

namespace AspNet.Security.OAuth.Infrastructure
{
    /// <summary>
    /// Registers an delegating handler to intercept HTTP requests made by the test application.
    /// </summary>
    internal sealed class HttpRequestInterceptionFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly HttpClientInterceptorOptions _options;

        internal HttpRequestInterceptionFilter(HttpClientInterceptorOptions options)
        {
            _options = options;
        }

        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            return builder =>
            {
                next(builder);
                builder.AdditionalHandlers.Add(_options.CreateHttpMessageHandler());
            };
        }
    }
}
