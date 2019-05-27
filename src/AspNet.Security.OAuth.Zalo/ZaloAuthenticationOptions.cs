﻿using System;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Zalo
{
    public class ZaloAuthenticationOptions : OAuthOptions
    {
        public ZaloAuthenticationOptions()
        {
            ClaimsIssuer = ZaloAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(ZaloAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = ZaloAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = ZaloAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = ZaloAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapCustomJson(ClaimTypes.DateOfBirth, user =>
            {
                return DateTime.TryParseExact(user.Value<string>("birthday"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOfBirth)
                    ? dateOfBirth.ToString("yyyy/MM/dd")
                    : string.Empty;
            });
        }
    }
}
