/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.QuickBooks;

/// <summary>
/// Contains constants specific to the <see cref="QuickBooksAuthenticationHandler"/>.
/// </summary>
public static class QuickBooksAuthenticationConstants
{
    public static class Claims
    {
        public const string AccountType = "urn:quickbooks:appenvrionment";
        public const string EmailVerified = "urn:quickbooks:email_verified";
        public const string PhoneNumberVerified = "urn:quickbooks:phone_number_verified";
    }
}
