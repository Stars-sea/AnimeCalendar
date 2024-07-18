using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Api.Storage;

namespace AnimeCalendar.Api.Test;

[TestClass]
public class ApiTestSetup : IAuthTokenStorage {

    // 获取: https://next.bgm.tv/demo/access-token
    private const string ACCESS_TOKEN = "...";

    public static User User { get; private set; }

    public Task<ulong?> GetExpires() => Task.FromResult((ulong?)ulong.MaxValue);

    public Task<string?> GetTokenAsync() => Task.FromResult(ACCESS_TOKEN);

    public Task<string?> RefreshTokenAsync() => Task.FromResult(ACCESS_TOKEN);

    public Task<bool> Store() => Task.FromResult(true);

    [TestInitialize]
    public async void Init() {
        BgmApiServices.UpdateTokenStorage(this);

        User = await BgmApiServices.UserApi.GetMe();
    }
}
