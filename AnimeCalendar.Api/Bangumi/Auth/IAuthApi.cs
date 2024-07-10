using Refit;

namespace AnimeCalendar.Api.Bangumi.Auth;

[Headers(BangumiApp.USER_AGENT)]
public interface IAuthApi {
    public const string AuthorizePageUrl = $"https://bgm.tv/oauth/authorize?response_type=code&client_id={BangumiApp.APP_ID}";

    [Post("/oauth/access_token")]
    public Task<AccessTokenResponse> RequestToken([Body] AccessTokenRequest _);

    [Post("/oauth/access_token")]
    public Task<AccessTokenResponse> RefreshToken([Body] AccessTokenRefreshRequest _);

    [Post("/oauth/token_status")]
    public Task<AccessTokenStatusResponse> RequestStatus([Body] AccessTokenStatusRequest _);
}
