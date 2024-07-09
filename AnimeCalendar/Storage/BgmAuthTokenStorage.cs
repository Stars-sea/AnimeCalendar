using AnimeCalendar.Api.Bangumi.Auth;
using AnimeCalendar.Api.Storage;

using Newtonsoft.Json;

using Refit;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Windows.Storage;

using static AnimeCalendar.Api.Bangumi.BangumiApiServices;

namespace AnimeCalendar.Storage;

#nullable enable
internal sealed class BgmAuthTokenStorage : IAuthTokenStorage {

    public const string FILE_NAME = "bgm_token.json";
    private static StorageFolder StorageFolder => ApplicationData.Current.LocalFolder;

    private AuthToken AuthToken { get; set; }
    public bool Available => AuthToken != null;

    private BgmAuthTokenStorage(AuthToken token) => AuthToken = token;

    public static async Task<BgmAuthTokenStorage?> Load() {
        IStorageItem item = await StorageFolder.TryGetItemAsync(FILE_NAME);
        if (item is not StorageFile file) return null;

        string content = await FileIO.ReadTextAsync(file);
        return JsonConvert.DeserializeObject<AuthToken>(content) switch { 
            AuthToken token => new BgmAuthTokenStorage(token),
            _               => null
        };
    }

    public static async Task<BgmAuthTokenStorage> Request(string callbackCode) {
        var response = await api.RequestAccessToken(AccessTokenRequest.Create(callbackCode));
        AuthToken token = new(response.AccessToken, response.RefreshToken, DateTime.Now);
        
        BgmAuthTokenStorage storage = new(token);
        await storage.Store();
        return storage;
    }

    public async Task<ulong?> GetExpires(CancellationToken? cancellation = null) {
        if (AuthToken == null) return null;

        // TODO: 减少访问次数

        var response = await api.RequestAccessTokenStatus(new(AuthToken.AccessToken));
        return response.Expires;
    }

    public Task<string?> GetTokenAsync(CancellationToken? cancellation = null)
        => Task.FromResult(AuthToken?.AccessToken);

    public async Task<string?> RefreshTokenAsync(CancellationToken? cancellation = null) {
        if (AuthToken == null) return null;

        var request  = AccessTokenRefreshRequest.Create(AuthToken.RefreshToken);
        var response = await api.RefreshAccessToken(request);

        AuthToken = AuthToken with {
            AccessToken = response.AccessToken,
        };
        await Store(cancellation).ConfigureAwait(false);
        return response.AccessToken;
    }

    public async Task<bool> Store(CancellationToken? cancellation = null) {
        if (AuthToken == null) return false;

        IStorageItem? item = await StorageFolder.TryGetItemAsync(FILE_NAME);
        IStorageFile  file = item switch {
            IStorageFile file1 => file1,
            _                  => await StorageFolder.CreateFileAsync(FILE_NAME)
        };

        string content = JsonConvert.SerializeObject(AuthToken);
        await FileIO.WriteTextAsync(file, content);

        return true;
    }

    #region Init
    private static readonly IAuthApi api;

    static BgmAuthTokenStorage() {
        RefitSettings settings = new(new NewtonsoftJsonContentSerializer(SerializerSettings));
        api = RestService.For<IAuthApi>(BASE_ADDRESS, settings);
    }
    #endregion
}
