using AnimeCalendar.Api.Bangumi.Auth;

using System;

namespace AnimeCalendar.Storage;

internal record AuthToken(
    string   AccessToken,
    string   RefreshToken,
    DateTime LastRefreshed,
    ulong    ExpiresIn
) {
    public DateTime DueTime => LastRefreshed + TimeSpan.FromSeconds(ExpiresIn);

    public bool IsExpired() => DateTime.Now >= DueTime;

    public AuthToken UpdateStatus(AccessTokenStatusResponse response)
        => this with {
            LastRefreshed = DateTime.Now,
            ExpiresIn     = response.Expires
        };

    public static AuthToken Create(AccessTokenResponse response)
        => new AuthToken(response.AccessToken, response.RefreshToken, DateTime.Now, response.ExpiresIn);
}