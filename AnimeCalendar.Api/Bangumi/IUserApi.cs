using AnimeCalendar.Api.Bangumi.Schemas;

using Refit;

namespace AnimeCalendar.Api.Bangumi;

[Headers(
    "Authorization: Bearer",
    BangumiApp.USER_AGENT
)]
public interface IUserApi {
    [Get("/v0/users/{username}")]
    Task<User> GetUser(string username);

    [Get("/v0/me")]
    Task<User> GetMe();
}
