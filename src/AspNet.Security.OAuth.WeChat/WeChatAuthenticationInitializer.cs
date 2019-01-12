using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.WeChat
{
    public class WeChatAuthenticationInitializer : IPostConfigureOptions<WeChatAuthenticationOptions>
    {
        private IMemoryCache _cache;

        /// <summary>
        /// Creates a new instance of the <see cref="WeChatAuthenticationInitializer"/> class.
        /// </summary>
        public WeChatAuthenticationInitializer([NotNull] IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// A ugly hack to deal with WeChat MP OAuth state length limitation.
        /// </summary>
        /// <param name="name">The authentication scheme associated with the handler instance.</param>
        /// <param name="options">The options instance to initialize.</param>
        public void PostConfigure([NotNull] string name, [NotNull] WeChatAuthenticationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.StateDataFormat = new StoreInCacheFormat(_cache, options.RemoteAuthenticationTimeout);
        }
    }
}