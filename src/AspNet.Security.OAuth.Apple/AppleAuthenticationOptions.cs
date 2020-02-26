/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Apple
{
    /// <summary>
    /// Defines a set of options used by <see cref="AppleAuthenticationHandler"/>.
    /// </summary>
    public class AppleAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppleAuthenticationOptions"/> class.
        /// </summary>
        public AppleAuthenticationOptions()
        {
            ClaimsIssuer = AppleAuthenticationDefaults.Issuer;
            CallbackPath = AppleAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = AppleAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AppleAuthenticationDefaults.TokenEndpoint;

            Events = new AppleAuthenticationEvents();

            Scope.Add("name");
            Scope.Add("email");
        }

        /// <summary>
        /// Gets or sets the period of time after which generated client secrets expire
        /// if <see cref="GenerateClientSecret"/> is set to <see langword="true"/>.
        /// </summary>
        /// <remarks>
        /// The default client secret lifetime is 6 months.
        /// </remarks>
        public TimeSpan ClientSecretExpiresAfter { get; set; } = TimeSpan.FromSeconds(15777000); // 6 months in seconds

        /// <summary>
        /// Gets or sets the <see cref="AppleAuthenticationEvents"/> used to handle authentication events.
        /// </summary>
        public new AppleAuthenticationEvents Events
        {
            get => (AppleAuthenticationEvents)base.Events;
            set => base.Events = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically generate a client secret.
        /// </summary>
        public bool GenerateClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the optional ID for your Sign in with Apple private key.
        /// </summary>
        public string? KeyId { get; set; }

        /// <summary>
        /// Gets or sets the default period of time to cache the Apple public key(s)
        /// retrieved from the endpoint specified by <see cref="PublicKeyEndpoint"/>.
        /// </summary>
        /// <remarks>
        /// The default public key cache lifetime is 15 minutes.
        /// </remarks>
        public TimeSpan PublicKeyCacheLifetime { get; set; } = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Gets or sets the URI the middleware will access to obtain the public key for
        /// validating tokens if <see cref="ValidateTokens"/> is <see langword="true"/>.
        /// </summary>
        public string PublicKeyEndpoint { get; set; } = AppleAuthenticationDefaults.PublicKeyEndpoint;

        /// <summary>
        /// Gets or sets an optional delegate to get the raw bytes of the client's private key
        /// which is passed the value of the <see cref="KeyId"/> property.
        /// </summary>
        /// <remarks>
        /// The private key should be in PKCS #8 (<c>.p8</c>) format.
        /// </remarks>
        public Func<string, Task<byte[]>>? PrivateKeyBytes { get; set; }

        /// <summary>
        /// Gets or sets the Team ID for your Apple Developer account.
        /// </summary>
        public string TeamId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the audience used for tokens.
        /// </summary>
        public string TokenAudience { get; set; } = AppleAuthenticationConstants.Audience;

        /// <summary>
        /// Gets or sets a value indicating whether to validate tokens using Apple's public key.
        /// </summary>
        public bool ValidateTokens { get; set; } = true;

        /// <inheritdoc />
        public override void Validate()
        {
            try
            {
                // HACK We want all of the base validation except for ClientSecret,
                // so rather than re-implement it all, catch the exception thrown
                // for that being null and only throw if we aren't auto-generating
                // the value. This does mean that three checks have to be re-implemented
                // because the won't be validated if the ClientSecret validation fails.
                base.Validate();
            }
            catch (ArgumentException ex) when (ex.ParamName == nameof(ClientSecret))
            {
                if (!GenerateClientSecret)
                {
                    throw;
                }
            }

            if (string.IsNullOrEmpty(AuthorizationEndpoint))
            {
                throw new ArgumentException($"The '{nameof(AuthorizationEndpoint)}' option must be provided.", nameof(AuthorizationEndpoint));
            }

            if (string.IsNullOrEmpty(TokenEndpoint))
            {
                throw new ArgumentException($"The '{nameof(TokenEndpoint)}' option must be provided.", nameof(TokenEndpoint));
            }

            if (!CallbackPath.HasValue)
            {
                throw new ArgumentException($"The '{nameof(CallbackPath)}' option must be provided.", nameof(CallbackPath));
            }

            if (GenerateClientSecret)
            {
                if (string.IsNullOrEmpty(KeyId))
                {
                    throw new ArgumentException($"The '{nameof(KeyId)}' option must be provided if the '{nameof(GenerateClientSecret)}' option is set to true.", nameof(KeyId));
                }

                if (string.IsNullOrEmpty(TeamId))
                {
                    throw new ArgumentException($"The '{nameof(TeamId)}' option must be provided if the '{nameof(GenerateClientSecret)}' option is set to true.", nameof(TeamId));
                }

                if (string.IsNullOrEmpty(TokenAudience))
                {
                    throw new ArgumentException($"The '{nameof(TokenAudience)}' option must be provided if the '{nameof(GenerateClientSecret)}' option is set to true.", nameof(TokenAudience));
                }

                if (ClientSecretExpiresAfter <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(ClientSecretExpiresAfter),
                        ClientSecretExpiresAfter,
                        $"The '{nameof(ClientSecretExpiresAfter)}' option must be a positive value if the '{nameof(GenerateClientSecret)}' option is set to true.");
                }
            }

            if (ValidateTokens)
            {
                if (string.IsNullOrEmpty(PublicKeyEndpoint))
                {
                    throw new ArgumentException($"The '{nameof(PublicKeyEndpoint)}' option must be provided if the '{nameof(ValidateTokens)}' option is set to true.", nameof(PublicKeyEndpoint));
                }
            }
        }
    }
}
