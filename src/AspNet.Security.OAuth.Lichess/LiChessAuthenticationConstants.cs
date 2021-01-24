/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Lichess
{
    /// <summary>
    /// Contains constants specific to the <see cref="LichessAuthenticationHandler"/>.
    /// </summary>
    public static class LiChessAuthenticationConstants
    {
        /// <summary>
        /// Lichess API Scopes
        /// <para>https://lichess.org/api#section/Authentication</para>
        /// </summary>
        public static class Scopes
        {
            /// <summary>
            /// Read your preferences
            /// </summary>
            public const string PreferencesRead = "preference:read";

            /// <summary>
            /// Write your preferences
            /// </summary>
            public const string PreferencesWrite = "preference:write";

            /// <summary>
            /// Read your email address
            /// </summary>
            public const string EmailRead = "email:read";

            /// <summary>
            /// Read incoming challenges
            /// </summary>
            public const string ChallengeRead = "challenge:read";

            /// <summary>
            /// Create, accept, decline challenges
            /// </summary>
            public const string ChallengeWrite = "challenge:write";

            /// <summary>
            /// Read private studies and broadcasts
            /// </summary>
            public const string StudyRead = "study:read";

            /// <summary>
            /// Create, update, delete studies and broadcasts
            /// </summary>
            public const string StudyWrite = "study:write";

            /// <summary>
            /// Create tournaments
            /// </summary>
            public const string TournamentWrite = "tournament:write";

            /// <summary>
            /// Read puzzle activity
            /// </summary>
            public const string PuzzleRead = "puzzle:read";

            /// <summary>
            /// Join, leave, and manage teams
            /// </summary>
            public const string TeamWrite = "team:write";

            /// <summary>
            /// Send private messages to other players
            /// </summary>
            public const string MessageWrite = "msg:write";

            /// <summary>
            /// Play with the Board API
            /// </summary>
            public const string BoardPlay = "board:play";

            /// <summary>
            /// Play with the Bot API.  Only for Bot Accounts.
            /// </summary>
            public const string BotPlay = "bot:play";
        }
    }
}
