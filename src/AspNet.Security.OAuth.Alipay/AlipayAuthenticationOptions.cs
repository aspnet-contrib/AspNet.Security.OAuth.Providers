/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using static AspNet.Security.OAuth.Alipay.AlipayAuthenticationConstants;

namespace AspNet.Security.OAuth.Alipay;

/// <summary>
/// Defines a set of options used by <see cref="AlipayAuthenticationHandler"/>.
/// </summary>
public class AlipayAuthenticationOptions : OAuthOptions
{
    public AlipayAuthenticationOptions()
    {
        ClaimsIssuer = AlipayAuthenticationDefaults.Issuer;
        CallbackPath = AlipayAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = AlipayAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = AlipayAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = AlipayAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("auth_user");

        ClaimActions.MapJsonKey(Claims.Avatar, "avatar");
        ClaimActions.MapJsonKey(Claims.City, "city");
        ClaimActions.MapJsonKey(Claims.Gender, "gender");
        ClaimActions.MapJsonKey(Claims.Nickname, "nick_name");
        ClaimActions.MapJsonKey(Claims.Province, "province");
        ClaimActions.MapJsonKey(Claims.OpenId, "open_id");

        // https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/pull/1131#discussion_r2657531257
        ClaimActions.MapJsonKey("urn:alipay:user_id", "user_id");
    }

    /// <summary>
    /// Gets or sets a value indicating whether to use certificate mode for signing calls.
    /// <para>https://opendocs.alipay.com/common/057k53?pathHash=e18d6f77#%E8%AF%81%E4%B9%A6%E6%A8%A1%E5%BC%8F</para>
    /// </summary>
    public bool UseCertificateSignatures { get; set; }

    /// <summary>
    /// Gets or sets the optional ID for your Sign in with Application Public Key Certificate SN(app_cert_sn).
    /// <para>https://opendocs.alipay.com/support/01raux</para>
    /// </summary>
    public string? ApplicationCertificateSnKeyId { get; set; }

    /// <summary>
    /// Gets or sets the optional ID for your Sign in with Alipay Root Certificate SN.
    /// <para>https://opendocs.alipay.com/support/01rauy</para>
    /// </summary>
    public string? RootCertificateSnKeyId { get; set; }

    /// <summary>
    /// Gets or sets an optional delegate to get the client's public key which is passed
    /// the value of the <see cref="ApplicationCertificateSnKeyId"/> or <see cref="RootCertificateSnKeyId"/> property and the <see cref="CancellationToken"/>
    /// associated with the current HTTP request.
    /// </summary>
    /// <remarks>
    /// The public key should be in PKCS #8 (<c>.p8</c>) format.
    /// </remarks>
    public Func<string, CancellationToken, Task<ReadOnlyMemory<char>>>? PublicKey { get; set; }

    /// <inheritdoc />
    public override void Validate()
    {
        base.Validate();

        if (UseCertificateSignatures)
        {
            if (string.IsNullOrEmpty(ApplicationCertificateSnKeyId))
            {
                throw new ArgumentException($"The '{nameof(ApplicationCertificateSnKeyId)}' option must be provided if the '{nameof(UseCertificateSignatures)}' option is set to true.", nameof(ApplicationCertificateSnKeyId));
            }

            if (string.IsNullOrEmpty(RootCertificateSnKeyId))
            {
                throw new ArgumentException($"The '{nameof(RootCertificateSnKeyId)}' option must be provided if the '{nameof(UseCertificateSignatures)}' option is set to true.", nameof(RootCertificateSnKeyId));
            }

            if (PublicKey == null)
            {
                throw new ArgumentException($"The '{nameof(PublicKey)}' option must be provided if the '{nameof(UseCertificateSignatures)}' option is set to true.", nameof(PublicKey));
            }
        }
    }
}
