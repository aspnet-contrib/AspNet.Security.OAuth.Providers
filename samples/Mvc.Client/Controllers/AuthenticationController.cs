/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Web;
using AspNet.Security.OAuth.Keycloak;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mvc.Client.Extensions;

namespace Mvc.Client.Controllers;

public class AuthenticationController : Controller
{
    private readonly IAuthenticationService authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    [HttpGet("~/signin")]
    public async Task<IActionResult> SignIn() => View("SignIn", await HttpContext.GetExternalProvidersAsync());

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
        return Challenge(new AuthenticationProperties { RedirectUri = "/" }, provider);
    }

    [HttpGet("~/signout")]
    [HttpPost("~/signout")]
    public async Task SignOutCurrentUser()
    {
        // Instruct the cookies middleware to delete the local cookie created
        // when the user agent is redirected from the external identity provider
        // after a successful authentication flow (e.g Google or Facebook).
        // return SignOut(new AuthenticationProperties { RedirectUri = "/" },
        // CookieAuthenticationDefaults.AuthenticationScheme);
        //var result = SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme);
        //return result;

        await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        await this.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }
}
