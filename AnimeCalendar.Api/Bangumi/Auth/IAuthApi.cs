using Refit;

namespace AnimeCalendar.Api.Bangumi.Auth;

[Headers(BangumiApp.USER_AGENT)]
internal interface IAuthApi {
    [Post("/oauth/access_token")]
    public Task<AccessTokenResponse> RequestToken([Body(BodySerializationMethod.Serialized, false)] AccessTokenRequest _);

    [Post("/oauth/access_token")]
    public Task<AccessTokenResponse> RefreshToken([Body] AccessTokenRefreshRequest _);

    [Post("/oauth/token_status")]
    public Task<AccessTokenStatusResponse> RequestStatus([Body] AccessTokenStatusRequest _);
}
