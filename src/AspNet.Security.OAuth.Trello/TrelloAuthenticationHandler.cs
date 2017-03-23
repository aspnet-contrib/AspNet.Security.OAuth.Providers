/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;

namespace AspNet.Security.OAuth.Trello {
    public class TrelloAuthenticationHandler : RemoteAuthenticationHandler<TrelloAuthenticationOptions> {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private const string StateCookie = "token";
        private readonly HttpClient _httpClient;
        private const string RequestTokenEndpoint = TrelloAuthenticationDefaults.RequestTokenEndpoint;
        private const string AuthenticationEndpoint = TrelloAuthenticationDefaults.AuthorizeTokenEndpoint;
        private const string AccessTokenEndpoint = TrelloAuthenticationDefaults.AccessTokenEndpoint;

        public TrelloAuthenticationHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected override async Task<AuthenticateResult> HandleRemoteAuthenticateAsync()
        {
            AuthenticationProperties properties = null;
            var query = Request.Query;
            var protectedRequestToken = Request.Cookies[StateCookie];

            var requestToken = Options.StateDataFormat.Unprotect(protectedRequestToken);

            if (requestToken == null)
            {
                return AuthenticateResult.Fail("Invalid state cookie.");
            }

            properties = requestToken.Properties;

            // REVIEW: see which of these are really errors

            var returnedToken = query["oauth_token"];
            if (StringValues.IsNullOrEmpty(returnedToken))
            {
                return AuthenticateResult.Fail("Missing oauth_token");
            }

            if (!string.Equals(returnedToken, requestToken.Token, StringComparison.Ordinal))
            {
                return AuthenticateResult.Fail("Unmatched token");
            }

            var oauthVerifier = query["oauth_verifier"];
            if (StringValues.IsNullOrEmpty(oauthVerifier))
            {
                return AuthenticateResult.Fail("Missing or blank oauth_verifier");
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps
            };

            Response.Cookies.Delete(StateCookie, cookieOptions);

            var accessToken = await ObtainAccessTokenAsync(requestToken, oauthVerifier);

            

            JObject user = null;
            user = await RetrieveUserDetailsAsync(accessToken);

            var identity = new ClaimsIdentity(Options.ClaimsIssuer);
            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, user["id"].ToString(), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, user["fullName"].ToString(), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, user["email"].ToString(), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:trello:name", user["fullName"].ToString(), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:trello:url", user["url"].ToString(), Options.ClaimsIssuer);


            if (Options.SaveTokens)
            {
                properties.StoreTokens(new[] {
                    new AuthenticationToken { Name = "access_token", Value = accessToken.Token },
                    new AuthenticationToken { Name = "access_token_secret", Value = accessToken.TokenSecret }
                });
            }
            return AuthenticateResult.Success(await CreateTicketAsync(identity, properties, accessToken, user));
        }

        protected virtual async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity, AuthenticationProperties properties, AccessToken token, JObject user)
        {

            var context = new TrelloCreatingTicketContext(Context, Options, token.UserId, token.UserName, token.Token, token.TokenSecret, user)
            {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            await Options.Events.CreatingTicket(context);

            if (context.Principal?.Identity == null)
            {
                return null;
            }

            return new AuthenticationTicket(context.Principal, context.Properties, Options.AuthenticationScheme);
        }

        protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var properties = new AuthenticationProperties(context.Properties);

            if (string.IsNullOrEmpty(properties.RedirectUri))
            {
                properties.RedirectUri = CurrentUri;
            }

            // If CallbackConfirmed is false, this will throw
            var requestToken = await ObtainRequestTokenAsync(BuildRedirectUri(Options.CallbackPath), properties);
            var TrelloAuthenticationEndpoint = AuthenticationEndpoint + "?name=" + UrlEncoder.Encode(Options.AppName) + "&oauth_token=" +  requestToken.Token;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                Expires = Options.SystemClock.UtcNow.Add(Options.RemoteAuthenticationTimeout),
            };

            Response.Cookies.Append(StateCookie, Options.StateDataFormat.Protect(requestToken), cookieOptions);

            var redirectContext = new TrelloRedirectToAuthorizationEndpointContext(
                Context, Options,
                properties, TrelloAuthenticationEndpoint);
            await Options.Events.RedirectToAuthorizationEndpoint(redirectContext);
            return true;
        }

        private async Task<RequestToken> ObtainRequestTokenAsync(string callBackUri, AuthenticationProperties properties)
        {
            Logger.ObtainRequestToken();

            var nonce = Guid.NewGuid().ToString("N");

            var authorizationParts = new SortedDictionary<string, string>
            {
                { "oauth_callback", callBackUri },
                { "oauth_consumer_key", Options.ConsumerKey },
                { "oauth_nonce", nonce },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", GenerateTimeStamp() },
                { "oauth_version", "1.0" }
            };

            var parameterBuilder = new StringBuilder();
            foreach (var authorizationKey in authorizationParts)
            {
                parameterBuilder.AppendFormat("{0}={1}&", UrlEncoder.Encode(authorizationKey.Key), UrlEncoder.Encode(authorizationKey.Value));
            }
            parameterBuilder.Length--;
            var parameterString = parameterBuilder.ToString();

            var canonicalizedRequestBuilder = new StringBuilder();
            canonicalizedRequestBuilder.Append(HttpMethod.Post.Method);
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(UrlEncoder.Encode(RequestTokenEndpoint));
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(UrlEncoder.Encode(parameterString));

            var signature = ComputeSignature(Options.ConsumerSecret, null, canonicalizedRequestBuilder.ToString());
            authorizationParts.Add("oauth_signature", signature);

            var authorizationHeaderBuilder = new StringBuilder();
            authorizationHeaderBuilder.Append("OAuth ");
            foreach (var authorizationPart in authorizationParts)
            {
                authorizationHeaderBuilder.AppendFormat(
                    "{0}=\"{1}\", ", authorizationPart.Key, UrlEncoder.Encode(authorizationPart.Value));
            }
            authorizationHeaderBuilder.Length = authorizationHeaderBuilder.Length - 2;

            var request = new HttpRequestMessage(HttpMethod.Post, RequestTokenEndpoint);
            request.Headers.Add("Authorization", authorizationHeaderBuilder.ToString());

            var response = await _httpClient.SendAsync(request, Context.RequestAborted);
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();

            var responseParameters = new FormCollection(new FormReader(responseText).ReadForm());
            if (!string.Equals(responseParameters["oauth_callback_confirmed"], "true", StringComparison.Ordinal))
            {
                throw new Exception("Trello oauth_callback_confirmed is not true.");
            }

            return new RequestToken { Token = Uri.UnescapeDataString(responseParameters["oauth_token"]), TokenSecret = Uri.UnescapeDataString(responseParameters["oauth_token_secret"]), CallbackConfirmed = true, Properties = properties };
        }

        private async Task<AccessToken> ObtainAccessTokenAsync(RequestToken token, string verifier)
        {
            // https://dev.Trello.com/docs/api/1/post/oauth/access_token

            Logger.ObtainAccessToken();

            var nonce = Guid.NewGuid().ToString("N");

            var authorizationParts = new SortedDictionary<string, string>
            {
                { "oauth_consumer_key", Options.ConsumerKey },
                { "oauth_nonce", nonce },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_token", token.Token },
                { "oauth_timestamp", GenerateTimeStamp() },
                { "oauth_verifier", verifier },
                { "oauth_version", "1.0" },
            };

            var parameterBuilder = new StringBuilder();
            foreach (var authorizationKey in authorizationParts)
            {
                parameterBuilder.AppendFormat("{0}={1}&", UrlEncoder.Encode(authorizationKey.Key), UrlEncoder.Encode(authorizationKey.Value));
            }
            parameterBuilder.Length--;
            var parameterString = parameterBuilder.ToString();

            var canonicalizedRequestBuilder = new StringBuilder();
            canonicalizedRequestBuilder.Append(HttpMethod.Post.Method);
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(UrlEncoder.Encode(AccessTokenEndpoint));
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(UrlEncoder.Encode(parameterString));

            var signature = ComputeSignature(Options.ConsumerSecret, token.TokenSecret, canonicalizedRequestBuilder.ToString());
            authorizationParts.Add("oauth_signature", signature);
            authorizationParts.Remove("oauth_verifier");

            var authorizationHeaderBuilder = new StringBuilder();
            authorizationHeaderBuilder.Append("OAuth ");
            foreach (var authorizationPart in authorizationParts)
            {
                authorizationHeaderBuilder.AppendFormat(
                    "{0}=\"{1}\", ", authorizationPart.Key, UrlEncoder.Encode(authorizationPart.Value));
            }
            authorizationHeaderBuilder.Length = authorizationHeaderBuilder.Length - 2;

            var request = new HttpRequestMessage(HttpMethod.Post, AccessTokenEndpoint);
            request.Headers.Add("Authorization", authorizationHeaderBuilder.ToString());

            var formPairs = new Dictionary<string, string>()
            {
                { "oauth_verifier", verifier },
            };

            request.Content = new FormUrlEncodedContent(formPairs);

            var response = await _httpClient.SendAsync(request, Context.RequestAborted);

            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("AccessToken request failed with a status code of " + response.StatusCode);
                response.EnsureSuccessStatusCode(); // throw
            }

            var responseText = await response.Content.ReadAsStringAsync();
            var responseParameters = new FormCollection(new FormReader(responseText).ReadForm());

            return new AccessToken
            {
                Token = Uri.UnescapeDataString(responseParameters["oauth_token"]),
                TokenSecret = Uri.UnescapeDataString(responseParameters["oauth_token_secret"])
            };
        }

        private async Task<JObject> RetrieveUserDetailsAsync(AccessToken accessToken)
        {
            Logger.RetrieveUserDetails();

            var nonce = Guid.NewGuid().ToString("N");

            var authorizationParts = new SortedDictionary<string, string>
                        {
                            { "oauth_consumer_key", Options.ConsumerKey },
                            { "oauth_nonce", nonce },
                            { "oauth_signature_method", "HMAC-SHA1" },
                            { "oauth_timestamp", GenerateTimeStamp() },
                            { "oauth_token", accessToken.Token },
                            { "oauth_version", "1.0" }
                        };

            var parameterBuilder = new StringBuilder();
            foreach (var authorizationKey in authorizationParts)
            {
                parameterBuilder.AppendFormat("{0}={1}&", UrlEncoder.Encode(authorizationKey.Key), UrlEncoder.Encode(authorizationKey.Value));
            }
            parameterBuilder.Length--;
            var parameterString = parameterBuilder.ToString();

            var resource_url = TrelloAuthenticationDefaults.UserInformationEndpoint;
            var canonicalizedRequestBuilder = new StringBuilder();
            canonicalizedRequestBuilder.Append(HttpMethod.Get.Method);
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(UrlEncoder.Encode(resource_url));
            canonicalizedRequestBuilder.Append("%26");
            canonicalizedRequestBuilder.Append(UrlEncoder.Encode(parameterString));

            var signature = ComputeSignature(Options.ConsumerSecret, accessToken.TokenSecret, canonicalizedRequestBuilder.ToString());
            authorizationParts.Add("oauth_signature", signature);

            var authorizationHeaderBuilder = new StringBuilder();
            authorizationHeaderBuilder.Append("OAuth ");
            foreach (var authorizationPart in authorizationParts)
            {
                authorizationHeaderBuilder.AppendFormat(
                    "{0}=\"{1}\", ", authorizationPart.Key, UrlEncoder.Encode(authorizationPart.Value));
            }
            authorizationHeaderBuilder.Length = authorizationHeaderBuilder.Length - 2;

            var request = new HttpRequestMessage(HttpMethod.Get, resource_url);
            request.Headers.Add("Authorization", authorizationHeaderBuilder.ToString());

            var response = await _httpClient.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("Email request failed with a status code of " + response.StatusCode);
                response.EnsureSuccessStatusCode(); // throw
            }
            var responseText = await response.Content.ReadAsStringAsync();

            var result = JObject.Parse(responseText);

            return result;
        }

        private static string GenerateTimeStamp()
        {
            var secondsSinceUnixEpocStart = DateTime.UtcNow - Epoch;
            return Convert.ToInt64(secondsSinceUnixEpocStart.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        private string ComputeSignature(string consumerSecret, string tokenSecret, string signatureData)
        {
            using (var algorithm = new HMACSHA1())
            {
                algorithm.Key = Encoding.ASCII.GetBytes(
                    string.Format(CultureInfo.InvariantCulture,
                        "{0}&{1}",
                        UrlEncoder.Encode(consumerSecret),
                        string.IsNullOrEmpty(tokenSecret) ? string.Empty : UrlEncoder.Encode(tokenSecret)));
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(signatureData));
                return Convert.ToBase64String(hash);
            }
        }


        //protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
        //    [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
        //    var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
        //    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

        //    var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
        //    response.EnsureSuccessStatusCode();

        //    var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

        //    identity.AddOptionalClaim(ClaimTypes.NameIdentifier, TrelloAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer);

        //    // TODO: Add any optional claims, eg
        //    //  .AddOptionalClaim("urn:trello:name", TrelloAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)

        //    var principal = new ClaimsPrincipal(identity);
        //    var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

        //    var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
        //    await Options.Events.CreatingTicket(context);

        //    return context.Ticket;
        //}
    }
}
