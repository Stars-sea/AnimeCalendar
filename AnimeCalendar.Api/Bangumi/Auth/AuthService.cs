using Refit;

using static AnimeCalendar.Api.Bangumi.BangumiApp;
using static AnimeCalendar.Api.Bangumi.BgmApiServices;

namespace AnimeCalendar.Api.Bangumi.Auth;

public static class AuthService {
    public const string AUTH_ADDRESS  = "https://bgm.tv";
    public const string AUTH_PAGE_URL = $"{AUTH_ADDRESS}/oauth/authorize?response_type=code&client_id={APP_ID}";

    private static readonly IAuthApi api = RestService.For<IAuthApi>(AUTH_ADDRESS, ServiceSettings);

    public static Task<AccessTokenResponse> Request(string callbackCode) 
        => api.RequestToken(new("authorization_code", APP_ID, APP_SECRET, callbackCode, REDIRECT_URI));

    public static Task<AccessTokenResponse> Refresh(string refreshToken)
        => api.RefreshToken(new("refresh_token", APP_ID, APP_SECRET, refreshToken, REDIRECT_URI));

    public static Task<AccessTokenStatusResponse> Status(string token)
        => api.RequestStatus(new(token));
}
