/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Threading.Tasks;
using AspNet.Security.OAuth.Shopify;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Mvc.Client.Extensions;

namespace Mvc.Client.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet("~/signin")]
        public async Task<IActionResult> SignIn()
        {
            var providers = await HttpContext.GetExternalProvidersAsync();
            return View("SignIn", providers);
        }

        [HttpPost("~/signin")]
        public async Task<IActionResult> SignIn([FromForm] string provider)
        {
            // Note: the "provider" parameter corresponds to the external
            // authentication provider choosen by the user agent.
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            if (!await HttpContext.IsProviderSupportedAsync(provider))
            {
                return BadRequest();
            }

            // Instruct the middleware corresponding to the requested external identity
            // provider to redirect the user agent to its own authorization endpoint.
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs
            if (provider != ShopifyAuthenticationDefaults.AuthenticationScheme)
            {
                return Challenge(new AuthenticationProperties
                {
                    RedirectUri = "/",
                }, provider);
            }


            // Shopify OAuth differs from most (all?) others in that you need to know the host name of the
            // shop in order to construct the authorization endpoint. This can be aquired either from the
            // user directly, or provided by the shopify app store during app install/activation.
            var authProps = new ShopifyAuthenticationProperties("the-cat-ball-test") // Put your shop name here.
            {
                RedirectUri = "/",

                // Override OAuthOptions.Scope. Must be fully formatted.
                //Scope = "read_customers,read_orders"

                // Set to true for a per-user, online-only, token. The retured token has an expiration date
                // and should not be persisted. An offline token is requested by default.
                //RequestPerUserToken = true
            };

            return Challenge(authProps, provider);
        }

        [HttpGet("~/signout"), HttpPost("~/signout")]
        public IActionResult SignOut()
        {
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}