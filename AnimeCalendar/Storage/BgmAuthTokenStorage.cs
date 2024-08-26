using AnimeCalendar.Api.Bangumi.Auth;
using AnimeCalendar.Api.Storage;

using Newtonsoft.Json;

using System;
using System.Threading.Tasks;

using Windows.Storage;

namespace AnimeCalendar.Storage;

internal sealed class BgmAuthTokenStorage : IAuthTokenStorage {

    public const string FILE_NAME = "bgm_token.json";
    public static StorageFolder StorageFolder => ApplicationData.Current.LocalFolder;

    private AuthToken AuthToken { get; set; }
    public  bool      IsDeleted { get; private set; }

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
        var response = await AuthService.Request(callbackCode);

        var token = AuthToken.Create(response);

        BgmAuthTokenStorage storage = new(token);
        await storage.Store();
        return storage;
    }

    public async Task<ulong?> GetExpires() {
        if (AuthToken == null) return null;

        var response = await AuthService.Status(AuthToken.AccessToken);
        AuthToken = AuthToken.UpdateStatus(response);
        return response.Expires;
    }

    Task<bool> IAuthTokenStorage.IsExpired() => Task.FromResult(IsDeleted || AuthToken.IsExpired());

    public Task<string?> GetTokenAsync()
        => Task.FromResult(AuthToken?.AccessToken);

    public async Task<string?> RefreshTokenAsync() {
        if (AuthToken == null) return null;

        var response = await AuthService.Refresh(AuthToken.RefreshToken);

        AuthToken = AuthToken.Create(response);
        await Store().ConfigureAwait(false);
        return response.AccessToken;
    }

    public async Task<bool> Store() {
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

    public async Task<bool> Delete() {
        IStorageItem? item = await StorageFolder.TryGetItemAsync(FILE_NAME);
        if (item != null)
            await item.DeleteAsync(StorageDeleteOption.PermanentDelete);
        IsDeleted = item == null || StorageFolder.TryGetItemAsync(FILE_NAME) == null;
        return IsDeleted;
    }
}
