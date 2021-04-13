# Integrating the Bitrix24 Provider

## Example

```csharp
services.AddAuthentication(options => /* Auth configuration */)
        .AddBitrix24(options =>
        {
            options.Domain = "my-domain";
            options.ClientId = "my-client-id";
            options.ClientSecret = "my-client-secret";

            // Optionally declare additional scopes or add claims
            options.Scope.Add(Scopes.BizProc);
            options.ClaimActions.MapJsonKey(ClaimTypes.Country, UserFields.PersonalCountry);
            options.ClaimActions.MapJsonKey(Claims.IsActive, UserFields.Active);;
        });
```

## Required Additional Settings

`Domain`

## Optional Settings

### Scopes

See [AspNet.Security.OAuth.Bitrix24.Bitrix24AuthenticationConstants.Scopes](/src/AspNet.Security.OAuth.Bitrix24/Bitrix24AuthenticationConstants.cs)

### ClaimActions
The fields of the user's profile to add as claims.
#### Default claims (already included)
```csharp
ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, UserFields.Id);
ClaimActions.MapJsonKey(ClaimTypes.Email, UserFields.Email);
ClaimActions.MapCustomJson(ClaimTypes.Name, json => string.Join(' ', json.GetProperty(UserFields.Name).GetString(), json.GetProperty(UserFields.SecondName).GetString(), json.GetProperty(UserFields.LastName).GetString()));
```

#### Available claims

``` C#
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
```
