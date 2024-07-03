namespace AnimeCalendar.Api.Bangumi;

// Fill the blanks...
internal static class BangumiApp {
    public const string APP_ID        = "...";
    public const string APP_SECRET    = "...";
    public const string REDIRECT_URI  = "ac://...";

    public const string USER_AGENT = "...";

    public static (string, string) Get() => (APP_ID, APP_SECRET);

    static BangumiApp() {
        throw new NotImplementedException("Fill the blanks and delete this function.");
    }
}
