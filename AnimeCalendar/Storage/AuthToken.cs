using System;

namespace AnimeCalendar.Storage;

internal record AuthToken(
    string AccessToken,
    string RefreshToken,
    DateTime LastRefreshed
) {
    public AuthToken(string AccessToken, string RefreshToken)
        : this(AccessToken, RefreshToken, DateTime.Now) { }
}