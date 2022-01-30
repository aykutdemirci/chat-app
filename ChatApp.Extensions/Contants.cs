namespace ChatApp.Extensions
{
    public static class SessionNames
    {
        /// <summary>
        /// Session'daki kullanıcı
        /// </summary>
        public const string CurrentUser = "CurrentUser";
    }

    public static class CacheKeys
    {
        /// <summary>
        /// Ön bellekteki mesajların okunduğu key değeri
        /// </summary>
        public const string LastMessages = "LastMessages";
    }
}
