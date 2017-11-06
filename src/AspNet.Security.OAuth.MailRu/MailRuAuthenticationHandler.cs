/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System;

namespace AspNet.Security.OAuth.MailRu
{
    public class MailRuAuthenticationHandler : OAuthHandler<MailRuAuthenticationOptions>
    {
        public MailRuAuthenticationHandler(
            [NotNull] IOptionsMonitor<MailRuAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync([NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties, [NotNull] OAuthTokenResponse tokens)
        {
            var queryParameters = new Dictionary<string, string>();
            queryParameters.Add("app_id", Options.ClientId);
            queryParameters.Add("method", "users.getInfo");
            queryParameters.Add("secure", "1");
            queryParameters.Add("session_key", tokens.AccessToken);

            //sign=hex_md5('app_id={client_id}method=users.getInfosecure=1session_key={access_token}{secret_key}')
            var signatureParameters = queryParameters.Select(x => string.Format("{0}={1}", x.Key, x.Value));
            var signature = GetMd5Hash(string.Concat(signatureParameters) + Options.ClientSecret);

            queryParameters.Add("sig", signature);

            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, queryParameters);

            var response = await Backchannel.GetAsync(address, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var payloadContainer = JArray.Parse(await response.Content.ReadAsStringAsync());
            var payload = payloadContainer.First as JObject;

            var principal = new ClaimsPrincipal(identity);

            var context = new OAuthCreatingTicketContext(principal,
                properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions(payload);

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        /// <summary>
        /// Returns MD5 Hash of input.
        /// </summary>
        /// <param name="input">The line.</param>
        private string GetMd5Hash(string input)
        {
            var provider = new MD5CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(input);
            bytes = provider.ComputeHash(bytes);
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
