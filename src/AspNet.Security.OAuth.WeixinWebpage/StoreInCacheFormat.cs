using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Security.OAuth.WeixinWebpage
{
    internal class StoreInCacheFormat : ISecureDataFormat<AuthenticationProperties>
    {
        private IMemoryCache _cache;
        private TimeSpan timeout;

        public StoreInCacheFormat(IMemoryCache cache, TimeSpan timeout)
        {
            this._cache = cache;
            this.timeout = timeout;
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

            _cache.Set(key, data, timeout);

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