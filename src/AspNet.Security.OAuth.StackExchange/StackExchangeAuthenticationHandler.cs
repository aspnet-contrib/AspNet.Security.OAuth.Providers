/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.StackExchange {
    public class StackExchangeAuthenticationHandler : OAuthHandler<StackExchangeAuthenticationOptions> {
        public StackExchangeAuthenticationHandler([NotNull] HttpClient client)
            : base(client) {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens) {
            
            //access tokens and request keys are passed in the querystring for StackExchange
            var queryParameters = new Dictionary<string, string>() {
                                      {"access_token", tokens.AccessToken},
                                      {"key", Options.RequestKey},
                                      {"site", Options.Site}
                                  };
            var requestUrl = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, queryParameters);

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            //content will be UTF-8 encoded and gzipped
            var payload = JObject.Parse(await ReadGzipUTF8ContentAsStringAsync(response.Content));

            //we cannot get the email claim from the stack exchange endpoint
            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, StackExchangeAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, StackExchangeAuthenticationHelper.GetDisplayName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Webpage, StackExchangeAuthenticationHelper.GetWebsiteUrl(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim("urn:stackexchange:link", StackExchangeAuthenticationHelper.GetLink(payload), Options.ClaimsIssuer);
            // TODO: Add any optional claims, eg
            //  .AddOptionalClaim("urn:stackexchange:name", StackExchangeAuthenticationHelper.GetName(payload), Options.ClaimsIssuer)

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri) {
            var tokenRequestParameters = new Dictionary<string, string> {
                { "client_id", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "client_secret", Options.ClientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            // Stack exchange always responsds with a payload of the form access_token=...&expires=1234
            //requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = requestContent;
            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                //convert the data to a json string instead
                var queryDictionary = QueryHelpers.ParseQuery(content);
                var payload = new JObject();
                foreach (var keyValuePair in queryDictionary) {
                    //assume we only have one of each parameter
                    payload[keyValuePair.Key] = keyValuePair.Value[0];
                }
                return OAuthTokenResponse.Success(payload);
            } else {
                Logger.LogError("An error occurred when retrieving an access token: the remote server " +
                               "returned a {Status} response with the following payload: {Headers} {Body}.",
                               /* Status: */ response.StatusCode,
                               /* Headers: */ response.Headers.ToString(),
                               /* Body: */ await response.Content.ReadAsStringAsync());
                return OAuthTokenResponse.Failed(new Exception("An error occurred when retrieving an access token"));
            }
        }


        private static async Task<string> ReadGzipUTF8ContentAsStringAsync(HttpContent responseContent) {
            using (var outputStream = new MemoryStream()) {
                var inputStream = await responseContent.ReadAsStreamAsync();
                using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress, false)) {
                    gzipStream.CopyTo(outputStream);
                }

                return Encoding.UTF8.GetString(outputStream.ToArray());
            }

        }
    }
}
