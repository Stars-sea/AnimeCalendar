namespace AnimeCalendar.Api.Storage;

#nullable enable
public interface IAuthTokenStorage {
    Task<ulong?> GetExpires(CancellationToken? cancellation = null);
    async Task<bool> IsExpired(CancellationToken? cancellation = null) {
        ulong? expires = await GetExpires(cancellation);
        return expires == null || expires <= 0;
    }

    Task<string?> GetTokenAsync(CancellationToken? cancellation = null);

    /// <summary>
    /// Refresh token and store it
    /// </summary>
    /// <returns>Auth token</returns>
    Task<string?> RefreshTokenAsync(CancellationToken? cancellation = null);

    Task<bool> Store(CancellationToken? cancellation = null);
}
