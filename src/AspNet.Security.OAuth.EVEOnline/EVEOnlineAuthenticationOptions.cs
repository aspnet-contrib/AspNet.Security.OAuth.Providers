/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.EVEOnline;

/// <summary>
/// Defines a set of options used by <see cref="EVEOnlineAuthenticationHandler"/>.
/// </summary>
public class EVEOnlineAuthenticationOptions : OAuthOptions
{
    public EVEOnlineAuthenticationOptions()
    {
        ClaimsIssuer = EVEOnlineAuthenticationDefaults.Issuer;
        CallbackPath = EVEOnlineAuthenticationDefaults.CallbackPath;

        Server = EVEOnlineAuthenticationServer.Tranquility;
    }

    /// <summary>
    /// Sets the server used when communicating with EVE Online
    /// (by default, <see cref="EVEOnlineAuthenticationServer.Tranquility"/>).
    /// </summary>
    public EVEOnlineAuthenticationServer Server
    {
        set
        {
            switch (value)
            {
                switch (value)
                {
                    case EVEOnlineAuthenticationServer.Tranquility:
                        AuthorizationEndpoint = EVEOnlineAuthenticationDefaults.Tranquility.AuthorizationEndpoint;
                        TokenEndpoint = EVEOnlineAuthenticationDefaults.Tranquility.TokenEndpoint;
                        break;

                    case EVEOnlineAuthenticationServer.Singularity:
                        AuthorizationEndpoint = EVEOnlineAuthenticationDefaults.Singularity.AuthorizationEndpoint;
                        TokenEndpoint = EVEOnlineAuthenticationDefaults.Singularity.TokenEndpoint;
                        break;

                    default:
                        throw new ArgumentException($"Server '{value}' is unsupported.", nameof(value));
                }
            }
        }
    }
}
