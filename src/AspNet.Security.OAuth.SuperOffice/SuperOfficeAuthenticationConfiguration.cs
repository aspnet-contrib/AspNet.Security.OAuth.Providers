/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace AspNet.Security.OAuth.SuperOffice
{
    /// <summary>
    /// Contains well-known configuration settings for SuperOffice.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Required for serialization.")]
    public class SuperOfficeAuthenticationConfiguration
    {
        public SuperOfficeAuthenticationConfiguration()
        {
        }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; } = string.Empty;

        [JsonPropertyName("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; } = string.Empty;

        [JsonPropertyName("token_endpoint")]
        public string TokenEndpoint { get; set; } = string.Empty;

        [JsonPropertyName("end_session_endpoint")]
        public string EndSessionEndpoint { get; set; } = string.Empty;

        [JsonPropertyName("jwks_uri")]
        public string JwksUri { get; set; } = string.Empty;

        [JsonPropertyName("scopes_supported")]
        public ICollection<string> Scopes { get; set; } = new Collection<string>();

        [JsonPropertyName("response_modes_supported")]
        public ICollection<string> ResponseModes { get; set; } = new Collection<string>();

        [JsonPropertyName("response_types_supported")]
        public ICollection<string> ResponseTypes { get; set; } = new Collection<string>();

        [JsonPropertyName("subject_types_supported")]
        public ICollection<string> SubjectTypes { get; set; } = new Collection<string>();

        [JsonPropertyName("id_token_signing_alg_values_supported")]
        public ICollection<string> IdTokenSigningAlgValues { get; set; } = new Collection<string>();

        [JsonPropertyName("token_endpoint_auth_methods_supported")]
        public ICollection<string> TokenEndpointAuthMethods { get; set; } = new Collection<string>();

        [JsonPropertyName("grant_types_supported")]
        public ICollection<string> GrantTypes { get; set; } = new Collection<string>();

        [JsonPropertyName("revocation_endpoint")]
        public string RevocationEndpoint { get; set; } = string.Empty;
    }
}
