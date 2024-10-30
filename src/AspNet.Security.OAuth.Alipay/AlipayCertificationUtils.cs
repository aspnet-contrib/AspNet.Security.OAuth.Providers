/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AspNet.Security.OAuth.Alipay;

/// <summary>
/// This class of code refers to the Alipay official SDK code.
/// See https://github.com/alipay/alipay-sdk-net-all/blob/1b7b73909954b107bddb6476dec68aafcc3f16e9/v2/AlipaySDKNet.Standard/Util/AntCertificationUtil.cs
/// See https://opendocs.alipay.com/common/056zub?pathHash=91c49771
/// </summary>
internal static class AlipayCertificationUtils
{
    internal static string GetCertSN([NotNull] string certFilePath)
    {
        using var cert = X509Certificate.CreateFromCertFile(certFilePath);
        return GetCertSN(cert);
    }

    internal static string GetCertSN([NotNull] X509Certificate cert)
    {
        var issuerDN = cert.Issuer.Replace(", ", ",", StringComparison.InvariantCulture);
        var input = issuerDN + new BigInteger(cert.GetSerialNumber());
#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
        var certSN = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(input))).ToLowerInvariant();
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms
        return certSN;
    }

    internal static string GetRootCertSN([NotNull] string rootCertPath, [NotNull] string signType = "RSA2")
    {
        var certSNs = new List<string>();
        var certCollection = new X509Certificate2Collection();
        certCollection.ImportFromPemFile(rootCertPath);

        foreach (var cert in certCollection)
        {
            if ((signType.StartsWith("RSA", StringComparison.Ordinal) && cert.SignatureAlgorithm.Value?.StartsWith("1.2.840.113549.1.1", StringComparison.Ordinal) == true) ||
                (signType.Equals("SM2", StringComparison.Ordinal) && cert.SignatureAlgorithm.Value?.StartsWith("1.2.156.10197.1.501", StringComparison.Ordinal) == true))
            {
                certSNs.Add(GetCertSN(cert));
            }
        }

        var rootCertSN = string.Join('_', certSNs);
        return rootCertSN;
    }
}
