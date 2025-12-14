/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AspNet.Security.OAuth.Alipay;

/// <summary>
/// Based on https://github.com/alipay/alipay-sdk-net-all/blob/b482d75d322e740760f9230d2a3859090af642a7/v2/AlipaySDKNet.Standard/Util/AntCertificationUtil.cs.
/// </summary>
internal static class AlipayCertificationUtil
{
    public static string GetCertSN(ReadOnlySpan<char> certPem)
    {
        using var cert = X509Certificate2.CreateFromPem(certPem);
        return GetCertSN(cert);
    }

    private static string GetCertSN(X509Certificate2 cert)
    {
        var issuerDN = cert.Issuer.Replace(", ", ",", StringComparison.Ordinal);
        var serialNumber = new BigInteger(cert.GetSerialNumber()).ToString(CultureInfo.InvariantCulture);

        if (issuerDN.StartsWith("CN", StringComparison.InvariantCulture))
        {
            return CalculateMd5(issuerDN + serialNumber);
        }

        var attributes = issuerDN.Split(',');
        Array.Reverse(attributes);
        return CalculateMd5(string.Join(',', attributes) + serialNumber);
    }

    public static string GetRootCertSN(ReadOnlySpan<char> certPem, string signType = "RSA2")
    {
        var certificates = new X509Certificate2Collection();
        certificates.ImportFromPem(certPem);
        var rootCertSN = string.Join('_', GetRootCertSN(certificates, signType));
        return rootCertSN;
    }

    private static IEnumerable<string> GetRootCertSN(X509Certificate2Collection certificates, string signType)
    {
        foreach (X509Certificate2 cert in certificates)
        {
            var signatureAlgorithm = cert.SignatureAlgorithm.Value;
            if (signatureAlgorithm != null)
            {
                if ((signType.StartsWith("RSA", StringComparison.OrdinalIgnoreCase) &&
                    signatureAlgorithm.StartsWith("1.2.840.113549.1.1", StringComparison.OrdinalIgnoreCase)) ||
                    (signType.StartsWith("SM2", StringComparison.OrdinalIgnoreCase) &&
                    signatureAlgorithm.StartsWith("1.2.156.10197.1.501", StringComparison.OrdinalIgnoreCase)))
                {
                    yield return GetCertSN(cert);
                }
            }
        }
    }

    private static string CalculateMd5(string s)
    {
        var buffer = Encoding.UTF8.GetBytes(s);
        Span<byte> hash = stackalloc byte[MD5.HashSizeInBytes];
#pragma warning disable CA5351
        MD5.HashData(buffer, hash);
#pragma warning restore CA5351
        return Convert.ToHexStringLower(hash);
    }
}
