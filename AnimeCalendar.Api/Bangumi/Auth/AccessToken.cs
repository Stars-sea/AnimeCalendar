using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeCalendar.Api.Bangumi.Auth;

public record AccessTokenRequest(
    string GrantType,
    string ClientId,
    string ClientSecret,
    string Code,
    string RedirectUri
// string State
)
{
    public static AccessTokenRequest Create(string code) => new(
        "authorization_code",
        BangumiApp.APP_ID,
        BangumiApp.APP_SECRET,
        code,
        BangumiApp.REDIRECT_URI
    );
}

public record AccessTokenRefreshRequest(
    string GrantType,
    string ClientId,
    string ClientSecret,
    string RefreshToken,
    string RedirectUri
)
{
    public static AccessTokenRefreshRequest Create(string refreshToken) => new(
        "refresh_token",
        BangumiApp.APP_ID,
        BangumiApp.APP_SECRET,
        refreshToken,
        BangumiApp.REDIRECT_URI
    );
}

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
