/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AspNet.Security.OAuth.Apple
{
    internal static class TestKeys
    {
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        internal static async Task<byte[]> GetPrivateKeyBytesAsync()
        {
            byte[] privateKey;

            if (IsWindows)
            {
                string content = await File.ReadAllTextAsync(Path.Combine("Apple", "test.p8"));

                if (content.StartsWith("-----BEGIN PRIVATE KEY-----", StringComparison.Ordinal))
                {
                    string[] keyLines = content.Split('\n');
                    content = string.Join(string.Empty, keyLines.Skip(1).Take(keyLines.Length - 2));
                }

                privateKey = Convert.FromBase64String(content);
            }
            else
            {
                privateKey = await File.ReadAllBytesAsync(Path.Combine("Apple", "test.pfx"));
            }

            return privateKey;
        }

        internal static string GetPrivateKeyPassword() => IsWindows ? string.Empty : "passw0rd";
    }
}
