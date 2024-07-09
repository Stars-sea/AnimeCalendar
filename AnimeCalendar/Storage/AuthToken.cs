using System;

namespace AnimeCalendar.Storage;

internal record AuthToken(
    string   AccessToken,
    string   RefreshToken,
    DateTime LastRefreshed
);