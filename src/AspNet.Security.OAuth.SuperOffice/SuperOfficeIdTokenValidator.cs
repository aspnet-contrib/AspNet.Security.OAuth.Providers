﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Represents the base class for validating SuperOffice ID tokens.
    /// </summary>
    public abstract class SuperOfficeIdTokenValidator
    {
        /// <summary>
        /// Validates the SuperOffice ID token associated with the specified context as an asynchronous operation.
        /// </summary>
        /// <param name="context">The context to validate the ID token for.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to validate the ID token.
        /// </returns>
        public abstract Task ValidateAsync([NotNull] SuperOfficeValidateIdTokenContext context);
    }
}
