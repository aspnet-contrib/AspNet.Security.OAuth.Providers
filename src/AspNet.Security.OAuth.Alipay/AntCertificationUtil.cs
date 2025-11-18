/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Buffers;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AspNet.Security.OAuth.Alipay;

/// <summary>
/// https://github.com/alipay/alipay-sdk-net-all/blob/master/v2/AlipaySDKNet.Standard/Util/AntCertificationUtil.cs
/// </summary>
internal static class AntCertificationUtil
{
    public static string GetCertSN(ReadOnlySpan<char> certContent)
    {
        using var cert = X509Certificate2.CreateFromPem(certContent);
        return GetCertSN(cert);
    }

    public static string GetCertSN(X509Certificate2 cert)
    {
        var issuerDN = cert.Issuer.Replace(", ", ",", StringComparison.InvariantCulture).AsSpan();
        var serialNumber = new BigInteger(cert.GetSerialNumber()).ToString(CultureInfo.InvariantCulture);
        var len = issuerDN.Length + serialNumber.Length;
        char[]? array = null;
        Span<char> chars = len <= StackallocByteThreshold ?
            stackalloc char[StackallocByteThreshold] :
            (array = ArrayPool<char>.Shared.Rent(len));
        try
        {
            if (issuerDN.StartsWith("CN", StringComparison.InvariantCulture))
            {
                issuerDN.CopyTo(chars);
                serialNumber.AsSpan().CopyTo(chars[issuerDN.Length..]);
                return CalculateMd5(chars[..len]);
            }

            List<Range> attributes = [];
            var issuerDNSplit = issuerDN.Split(',');
            while (issuerDNSplit.MoveNext())
            {
                attributes.Add(issuerDNSplit.Current);
            }

            Span<char> charsTemp = chars;
            for (var i = attributes.Count - 1; i >= 0; i--) // attributes.Reverse()
            {
                var it = issuerDN[attributes[i]];
                it.CopyTo(charsTemp);
                charsTemp = charsTemp[it.Length..];
                if (i != 0)
                {
                    charsTemp[0] = ',';
                    charsTemp = charsTemp[1..];
                }
            }

            serialNumber.AsSpan().CopyTo(charsTemp);
            return CalculateMd5(chars[..len]);
        }
        finally
        {
            if (array != null)
            {
                ArrayPool<char>.Shared.Return(array);
            }
        }
    }

    public static string GetRootCertSN(ReadOnlySpan<char> rootCertContent, string signType = "RSA2")
    {
        var rootCertSN = string.Join('_', GetRootCertSNCore(rootCertContent, signType));
        return rootCertSN;
    }

    private static IEnumerable<string> GetRootCertSNCore(X509Certificate2Collection x509Certificates, string signType)
    {
        foreach (X509Certificate2 cert in x509Certificates)
        {
            var signatureAlgorithm = cert.SignatureAlgorithm.Value;
            if (signatureAlgorithm != null)
            {
                if ((signType.StartsWith("RSA", StringComparison.InvariantCultureIgnoreCase) &&
                    signatureAlgorithm.StartsWith("1.2.840.113549.1.1", StringComparison.InvariantCultureIgnoreCase)) ||
                    (signType.StartsWith("SM2", StringComparison.InvariantCultureIgnoreCase) &&
                    signatureAlgorithm.StartsWith("1.2.156.10197.1.501", StringComparison.InvariantCultureIgnoreCase)))
                {
                    yield return GetCertSN(cert);
                }
            }
        }
    }

    private static IEnumerable<string> GetRootCertSNCore(ReadOnlySpan<char> rootCertContent, string signType)
    {
        X509Certificate2Collection x509Certificates = [];
        x509Certificates.ImportFromPem(rootCertContent);
        return GetRootCertSNCore(x509Certificates, signType);
    }

    /// <summary>
    /// https://github.com/dotnet/runtime/blob/v9.0.8/src/libraries/System.Text.Json/Common/JsonConstants.cs#L12
    /// </summary>
    private const int StackallocByteThreshold = 256;

    private static string CalculateMd5(ReadOnlySpan<char> chars)
    {
        var lenU8 = Encoding.UTF8.GetMaxByteCount(chars.Length);
        byte[]? array = null;
        Span<byte> bytes = lenU8 <= StackallocByteThreshold ?
            stackalloc byte[StackallocByteThreshold] :
            (array = ArrayPool<byte>.Shared.Rent(lenU8));
        try
        {
            Encoding.UTF8.TryGetBytes(chars, bytes, out var bytesWritten);
            bytes = bytes[..bytesWritten];

            Span<byte> hash = stackalloc byte[MD5.HashSizeInBytes];
#pragma warning disable CA5351
            MD5.HashData(bytes, hash);
#pragma warning restore CA5351

            return Convert.ToHexStringLower(hash);
        }
        finally
        {
            if (array != null)
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }
    }
}
