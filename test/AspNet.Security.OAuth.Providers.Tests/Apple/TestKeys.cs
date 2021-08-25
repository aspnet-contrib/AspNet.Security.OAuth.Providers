/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AspNet.Security.OAuth.Apple
{
    internal static class TestKeys
    {
        internal static async Task<string> GetPrivateKeyAsync(CancellationToken cancellationToken = default)
            => await File.ReadAllTextAsync(Path.Combine("Apple", "test.p8"), cancellationToken);
    }
}
