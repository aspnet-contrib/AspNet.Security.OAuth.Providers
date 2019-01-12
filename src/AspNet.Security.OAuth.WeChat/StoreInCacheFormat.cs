using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace AspNet.Security.OAuth.WeChat
{
    internal class StoreInCacheFormat : ISecureDataFormat<AuthenticationProperties>
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _timeout;

        public StoreInCacheFormat(IMemoryCache cache, TimeSpan timeout)
        {
            _cache = cache;
            _timeout = timeout;
        }

        public string Protect(AuthenticationProperties data)
        {
            return Protect(data, purpose: null);
        }

        public string Protect(AuthenticationProperties data, string purpose)
        {
            Guid guid = Guid.NewGuid();
            var guidStr = guid.ToString();
            string key = guidStr;
            if (!string.IsNullOrEmpty(purpose))
            {
                key = purpose + key;
            }

            _cache.Set(key, data, _timeout);

            return guidStr;
        }

        public AuthenticationProperties Unprotect(string protectedText)
        {
            return Unprotect(protectedText, purpose: null);
        }

        public AuthenticationProperties Unprotect(string protectedText, string purpose)
        {
            var key = protectedText;
            if (!string.IsNullOrEmpty(purpose))
            {
                key = purpose + key;
            }

            var props = _cache.Get<AuthenticationProperties>(key);

            //Weixin may return code multiple times
            //copy props to prevent failure coursed by the changes to props.
            return new AuthenticationProperties(new Dictionary<string, string>(props.Items));
        }
    }
}