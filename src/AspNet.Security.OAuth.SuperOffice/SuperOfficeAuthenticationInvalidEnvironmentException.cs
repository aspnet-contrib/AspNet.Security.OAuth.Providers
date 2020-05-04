/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */
using System;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// The exception that is thrown when the <see cref="SuperOfficeAuthenticationOptions"/> Environment property is not valid.
    /// </summary>
    public class SuperOfficeAuthenticationInvalidEnvironmentException : Exception
    {
        public SuperOfficeAuthenticationInvalidEnvironmentException()
        {
        }

        public SuperOfficeAuthenticationInvalidEnvironmentException(string message)
            : base(message)
        {
        }

        public SuperOfficeAuthenticationInvalidEnvironmentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
