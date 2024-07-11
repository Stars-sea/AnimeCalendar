namespace AnimeCalendar.Api.Bangumi.Auth;

public record AccessTokenRequest(
    string GrantType,
    string ClientId,
    string ClientSecret,
    string Code,
    string RedirectUri
// string State
);

public record AccessTokenRefreshRequest(
    string GrantType,
    string ClientId,
    string ClientSecret,
    string RefreshToken,
    string RedirectUri
);

public record AccessTokenStatusRequest(string AccessToken);

public record AccessTokenResponse(
    string AccessToken,
    ulong ExpiresIn,
    string TokenType,
    string Scope,
    int UserId,
    string RefreshToken
);

public record AccessTokenStatusResponse(
    string AccessToken,
    string ClientId,
    ulong Expires,
    int UserId,
    string Scope
);
