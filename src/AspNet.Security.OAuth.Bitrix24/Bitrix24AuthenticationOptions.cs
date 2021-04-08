/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Bitrix24.Bitrix24AuthenticationConstants;

namespace AspNet.Security.OAuth.Bitrix24
{
    /// <summary>
    /// Defines a set of options used by <see cref="Bitrix24AuthenticationHandler"/>.
    /// </summary>
    public class Bitrix24AuthenticationOptions : OAuthOptions
    {
        private string? tenantHostName;

        public string? TenantHostName
        {
            get
            {
                return tenantHostName;
            }

            set
            {
                tenantHostName = value;
                AuthorizationEndpoint = string.Format(CultureInfo.InvariantCulture, Bitrix24AuthenticationDefaults.AuthorizationEndpoint, value);
                TokenEndpoint = string.Format(CultureInfo.InvariantCulture, Bitrix24AuthenticationDefaults.TokenEndpoint, value);
                UserInformationEndpoint = string.Format(CultureInfo.InvariantCulture, Bitrix24AuthenticationDefaults.UserInformationEndpoint, value);
            }
        }

        public Bitrix24AuthenticationOptions()
        {
            ClaimsIssuer = Bitrix24AuthenticationDefaults.Issuer;
            CallbackPath = Bitrix24AuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = Bitrix24AuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = Bitrix24AuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = Bitrix24AuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, UserFields.Id);
            ClaimActions.MapJsonKey(ClaimTypes.Email, UserFields.Email);
            ClaimActions.MapCustomJson(ClaimTypes.Name, json => string.Join(' ', json.GetProperty(UserFields.Name).GetString(), json.GetProperty(UserFields.SecondName).GetString(), json.GetProperty(UserFields.LastName).GetString()));
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, UserFields.Name);
            ClaimActions.MapJsonKey(Claims.MiddleName, UserFields.SecondName);
            ClaimActions.MapJsonKey(ClaimTypes.Surname, UserFields.LastName);
            ClaimActions.MapJsonKey(ClaimTypes.Gender, UserFields.PersonalGender);
            ClaimActions.MapJsonKey(ClaimTypes.Webpage, UserFields.PersonalWww);
            ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, UserFields.PersonalBirthday);
            ClaimActions.MapJsonKey(ClaimTypes.HomePhone, UserFields.PersonalPhone);
            ClaimActions.MapJsonKey(ClaimTypes.MobilePhone, UserFields.PersonalMobile);
            ClaimActions.MapJsonKey(ClaimTypes.StreetAddress, UserFields.PersonalStreet);
            ClaimActions.MapJsonKey(ClaimTypes.StateOrProvince, UserFields.PersonalState);
            ClaimActions.MapJsonKey(ClaimTypes.PostalCode, UserFields.PersonalZip);
            ClaimActions.MapJsonKey(ClaimTypes.Country, UserFields.PersonalCountry);

            ClaimActions.MapJsonKey(Claims.IsActive, UserFields.Active);
            ClaimActions.MapJsonKey(Claims.Profession, UserFields.PersonalProfession);
            ClaimActions.MapJsonKey(Claims.Photo, UserFields.PersonalPhoto);
            ClaimActions.MapJsonKey(Claims.ICQ, UserFields.PersonalIcq);
            ClaimActions.MapJsonKey(Claims.Fax, UserFields.PersonalFax);
            ClaimActions.MapJsonKey(Claims.Pager, UserFields.PersonalPager);
            ClaimActions.MapJsonKey(Claims.City, UserFields.PersonalCity);
            ClaimActions.MapJsonKey(Claims.Company, UserFields.WorkCompany);
            ClaimActions.MapJsonKey(Claims.Position, UserFields.WorkPosition);
            ClaimActions.MapJsonKey(Claims.UfDepartment, UserFields.UfDepartment);
            ClaimActions.MapJsonKey(Claims.UfInterests, UserFields.UfInterests);
            ClaimActions.MapJsonKey(Claims.UfSkills, UserFields.UfSkills);
            ClaimActions.MapJsonKey(Claims.UfWebSites, UserFields.UfWebSites);
            ClaimActions.MapJsonKey(Claims.UfXing, UserFields.UfXing);
            ClaimActions.MapJsonKey(Claims.UfLinkedin, UserFields.UfLinkedin);
            ClaimActions.MapJsonKey(Claims.UfFacebook, UserFields.UfFacebook);
            ClaimActions.MapJsonKey(Claims.UfTwitter, UserFields.UfTwitter);
            ClaimActions.MapJsonKey(Claims.UfSkype, UserFields.UfSkype);
            ClaimActions.MapJsonKey(Claims.UfDistrict, UserFields.UfDistrict);
            ClaimActions.MapJsonKey(Claims.UfPhoneInner, UserFields.UfPhoneInner);
        }
    }
}
