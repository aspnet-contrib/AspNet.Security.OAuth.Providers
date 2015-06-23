using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;
using Mvc.Client.Extensions;

namespace Mvc.Client.Controllers {
    public class AuthenticationController : Controller {
        [HttpGet("~/signin")]
        public IActionResult SignIn() => View("SignIn", Context.GetExternalProviders());

        [HttpPost("~/signin")]
        public IActionResult SignIn([FromForm] string provider) {
            // Note: the "provider" parameter corresponds to the external
            // authentication provider choosen by the user agent.
            if (string.IsNullOrWhiteSpace(provider)) {
                return HttpBadRequest();
            }

            if (!Context.IsProviderSupported(provider)) {
                return HttpBadRequest();
            }
            
            // Instruct the middleware corresponding to the requested external identity
            // provider to redirect the user agent to its own authorization endpoint.
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs
            return new ChallengeResult(provider, new AuthenticationProperties { RedirectUri = "/" });
        }

        [HttpGet("~/signout")]
        [HttpPost("~/signout")]
        public async Task<IActionResult> SignOut() {
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            await Context.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}