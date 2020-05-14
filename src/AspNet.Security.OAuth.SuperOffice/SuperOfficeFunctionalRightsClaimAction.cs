/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace AspNet.Security.OAuth.SuperOffice
{
    internal sealed class SuperOfficeFunctionalRightsClaimAction : ClaimAction
    {
        private readonly SuperOfficeAuthenticationOptions _options;

        internal SuperOfficeFunctionalRightsClaimAction(SuperOfficeAuthenticationOptions options)
            : base(ClaimTypes.Email, ClaimValueTypes.String)
        {
            _options = options;
        }

        public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
        {
            if (_options.IncludeFunctionalRightsAsClaims && userData.ValueKind == JsonValueKind.Object)
            {
                foreach (var item in userData.EnumerateObject())
                {
                    if (item.Name.Equals(SuperOfficeAuthenticationConstants.PrincipalNames.FunctionRights, StringComparison.OrdinalIgnoreCase))
                    {
                        if (item.Value.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var functionRight in item.Value.EnumerateArray())
                            {
                                identity.AddClaim(new Claim(SuperOfficeAuthenticationConstants.PrincipalNames.FunctionRights, functionRight.GetString()));
                            }
                        }
                    }
                }
            }
        }
    }
}
