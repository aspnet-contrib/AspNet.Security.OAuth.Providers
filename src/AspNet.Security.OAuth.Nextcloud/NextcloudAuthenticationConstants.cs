namespace AspNet.Security.OAuth.Nextcloud
{
    public static class NextcloudAuthenticationConstants
    {
        public static class Claims
        {
            public const string Groups = "urn:nextcloud:groups";
            public const string DisplayName = "urn:nextcloud:displayname";
            public const string Enabled = "urn:nextcloud:enabled";
            public const string Language = "urn:nextcloud:language";
            public const string Locale = "urn:nextcloud:locale";
        }
    }
}
