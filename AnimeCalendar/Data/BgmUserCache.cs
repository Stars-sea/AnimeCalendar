using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Api.Storage;
using AnimeCalendar.Storage;

using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Threading.Tasks;

namespace AnimeCalendar.Data;

internal sealed partial class BgmUserCache : ObservableObject {
    public static readonly BgmUserCache Instance = new();

    private BgmUserCache() { }

    [ObservableProperty]
    private IAuthTokenStorage? tokenStorage;

    [ObservableProperty]
    private User? user;

    public static event Action<IAuthTokenStorage?>? TokenChanged;
    public static event Action<User?>? UserChanged;

    public static async Task<bool> IsTokenNullOrExpired()
        => Instance.TokenStorage == null || await Instance.TokenStorage.IsExpired();

    public static async Task LoadTokenAsync() {
        Instance.TokenStorage = await BgmAuthTokenStorage.Load();

        if (Instance.TokenStorage != null && await Instance.TokenStorage.IsExpired())
            await Instance.TokenStorage.RefreshTokenAsync();
    }

    public async void UpdateUserAsync()
        => User = !await IsTokenNullOrExpired()
            ? await BgmApiServices.UserApi.GetMe()
            : null;

    partial void OnTokenStorageChanged(IAuthTokenStorage? value) {
        BgmApiServices.UpdateTokenStorage(value);

        UpdateUserAsync();

        TokenChanged?.Invoke(value);
    }

    partial void OnUserChanged(User? value) => UserChanged?.Invoke(value);
}