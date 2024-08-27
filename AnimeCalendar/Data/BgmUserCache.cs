using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Api.Storage;
using AnimeCalendar.Storage;

using CommunityToolkit.Mvvm.ComponentModel;

using System.Threading.Tasks;

namespace AnimeCalendar.Data;

internal sealed partial class BgmUserCache : ObservableRecipient {
    public static readonly BgmUserCache Instance = new();

    private BgmUserCache() { }

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private IAuthTokenStorage? tokenStorage;

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private User? user;

    public static async Task<bool> IsTokenNullOrExpired()
        => Instance.TokenStorage == null || await Instance.TokenStorage.IsExpired();

    public static async Task LoadTokenAsync() {
        Instance.TokenStorage = await BgmAuthTokenStorage.Load();

        try {
            if (Instance.TokenStorage != null && await Instance.TokenStorage.IsExpired())
                await Instance.TokenStorage.RefreshTokenAsync();
        }
        catch {
            if (Instance.TokenStorage != null) {
                await Instance.TokenStorage.Delete();
                Instance.TokenStorage = null;
            }
            throw;
        }
    }

    public async ValueTask LogoutAsync() {
        if (TokenStorage == null) return;

        await TokenStorage.Delete();
        TokenStorage = null;
    }

    public async Task UpdateUserAsync()
        => User = !await IsTokenNullOrExpired()
            ? await BgmApiServices.UserApi.GetMe()
            : null;

    partial void OnTokenStorageChanged(IAuthTokenStorage? oldValue, IAuthTokenStorage? newValue) {
        BgmApiServices.UpdateTokenStorage(newValue);
        UpdateUserAsync().ConfigureAwait(false);
    }
}
