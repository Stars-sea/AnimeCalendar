namespace AnimeCalendar.Api.Storage;

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

    async Task<string?> RefreshIfExpired() {
        if (await IsExpired())
            return await RefreshTokenAsync();
        return await GetTokenAsync();
    }

    Task<bool> Store();
}
