/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AspNet.Security.OAuth.MailRu
{
    public class MailRuAuthenticationHandler : OAuthHandler<MailRuAuthenticationOptions>
    {
        public MailRuAuthenticationHandler([NotNull] HttpClient client)
            : base(client)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var prms = new Dictionary<string, string>
            {
                ["app_id"] = Options.ClientId,
                ["method"] = "users.getInfo",
                ["format"] = "json",
                ["secure"] = "1",
                ["session_key"] = tokens.AccessToken,
            };

            var url = BuildUrlWithSignature(Options.UserInformationEndpoint, prms, Options.SignKey);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var payload = (JObject)JArray.Parse(await response.Content.ReadAsStringAsync())[0];

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, MailRuAuthenticationHelper.GetIdentifier(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Name, MailRuAuthenticationHelper.GetNickname(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Surname, MailRuAuthenticationHelper.GetFamilyName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.GivenName, MailRuAuthenticationHelper.GetGivenName(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Webpage, MailRuAuthenticationHelper.GetLink(payload), Options.ClaimsIssuer)
                    .AddOptionalClaim(ClaimTypes.Email, MailRuAuthenticationHelper.GetEmail(payload), Options.ClaimsIssuer);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

            var context = new OAuthCreatingTicketContext(ticket, Context, Options, Backchannel, tokens, payload);
            await Options.Events.CreatingTicket(context);

            return context.Ticket;
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync([NotNull] string code, [NotNull] string redirectUri)
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Options.ClientId}:{Options.ClientSecret}"));

            var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["redirect_uri"] = redirectUri,
                ["code"] = code
            });

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            return OAuthTokenResponse.Success(payload);
        }

        protected virtual string BuildUrlWithSignature(string baseUrl, IDictionary<string, string> parameters, string signKey)
        {
            // Mail.ru REST API rules (RU manual: http://api.mail.ru/docs/guides/restapi/):
            //   1. method name (users.getInfo) is passed as param
            //   2. MD5 hash computed over all params (ordered alphabetically),
            //        concatenated 'name1=value1name2=value2...signkey' without separator (without &)
            //   3. MD5 Hash value passed as 'sig' param

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var sigText = string.Concat(parameters.OrderBy(x => x.Key).Select(x => x.Key + "=" + x.Value)) + signKey;
                var sigBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(sigText));
                var sig = BitConverter.ToString(sigBytes).Replace("-", string.Empty).ToLowerInvariant();

                var result = QueryHelpers.AddQueryString(baseUrl, parameters);
                return QueryHelpers.AddQueryString(result, "sig", sig);
            }
        }
    }
}