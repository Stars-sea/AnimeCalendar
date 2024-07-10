namespace AnimeCalendar.Api.Storage;

#nullable enable
public interface IAuthTokenStorage {
    Task<ulong?> GetExpires();
    async Task<bool> IsExpired() {
        ulong? expires = await GetExpires();
        return expires == null || expires <= 0;
    }

    Task<string?> GetTokenAsync();

    /// <summary>
    /// Refresh token and store it
    /// </summary>
    /// <returns>Auth token</returns>
    Task<string?> RefreshTokenAsync();

    Task<bool> Store();
}
