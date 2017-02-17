/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Salesforce
{
    /// <summary>
    /// Defines a list of environments used to determine the appropriate
    /// OAuth2 endpoints when communicating with Salesforce.
    /// </summary>
    public enum SalesforceAuthenticationEnvironment
    {
        /// <summary>
        /// Use login.salesforce.com in the OAuth2 endpoints.
        /// </summary>
        Production = 0,

        /// <summary>
        /// Uses test.salesforce.com in the OAuth2 endpoints.
        /// </summary>
        Test = 1
    }
}
