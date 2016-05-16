using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.Security.OAuth.Salesforce
{
    /// <summary>
    /// Defines a list of environments used to determine the appropriate
    /// OAuth endpoints when communicating with Salesforce.
    /// </summary>
    public enum SalesforceEnvironment
    {
        /// <summary>
        /// Use login.salesforce.com in the OAuth endpoints
        /// </summary>
        Production = 0,

        /// <summary>
        /// Uses test.salesforce.com in the OAuth endpoints
        /// </summary>
        Test = 1
    }
}
