using AnimeCalendar.Api.Bangumi.Auth;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Api.Storage;
using AnimeCalendar.Controls.Cards;
using AnimeCalendar.Data;
using AnimeCalendar.Storage;
using AnimeCalendar.Utils;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

using System;
using System.Threading;
using System.Threading.Tasks;

using Launcher = Windows.System.Launcher;

namespace AnimeCalendar.Pages;

public sealed partial class AccountSettingsPage : Page, IRecipient<PropertyChangedMessage<User?>> {
    public AccountSettingsPage() {
        InitializeComponent();
        Loaded += OnLoaded;
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    public void Receive(PropertyChangedMessage<User?> message)
        => OnBgmUserChanged(message.NewValue);

    private void OnBgmUserChanged(User? value) {
        if (value == null) {
            BgmCard.Content    = new TextBlock { Text = "在浏览器上登录 Bangumi 并授权", VerticalAlignment = VerticalAlignment.Center };
            BgmCard.IconSource = new BitmapImage(new Uri("https://bgm.tv/img/favicon.ico"));
        }
        else {
            BgmCard.Content    = new BgmUserCard() { User = value };
            BgmCard.IconSource = new BitmapImage(value.Avatar.Medium);
        }
    }

    private async Task<IAuthTokenStorage?> StartBgmAuth() {
        ContentDialog dialog = new() {
            XamlRoot        = XamlRoot,
            Title           = "Bangumi 授权",
            Content         = "你将会跳转到浏览器, 授权后此对话框将会自行关闭",
            CloseButtonText = "取消",
            DefaultButton   = ContentDialogButton.Close
        };

        CancellationTokenSource readCallbackCancelSource = new();

        var showDialog   = dialog.ShowAsync();
        var readCallback = App.CallbackUri.ReadAsync(readCallbackCancelSource.Token);

        await Task.Delay(800);
        await Windows.System.Launcher.LaunchUriAsync(new Uri(AuthService.AUTH_PAGE_URL));

        Task<object?> result = await Task.WhenAny(showDialog.WrapTask(), readCallback.WrapTask());
        if (await result is not CallbackUri uri) {
            readCallbackCancelSource.Cancel();
            App.MainWindow.Pop(PopInfo.Info("Bangumi 授权", "操作已取消"));
            return null;
        }

        dialog.Hide();

        try {
            App.MainWindow.Pop(PopInfo.Info("Bangumi 授权", "拉取令牌中..."));
            return await BgmAuthTokenStorage.Request(uri.Code);
        }
        catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("Bangumi 授权", "无法取得令牌", ex));
        }
        return null;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
        => OnBgmUserChanged(BgmUserCache.Instance.User);

    private async void BangumiLoginCard_Click(object sender, RoutedEventArgs e) {
        BgmUserCache cache = BgmUserCache.Instance;

        if (await BgmUserCache.IsTokenNullOrExpired()) {
            if (await StartBgmAuth() is IAuthTokenStorage authToken)
                cache.TokenStorage = authToken;
        }
        else if (cache.User != null) {
            await OpenLinkAsync();
        }
    }

    private void MikanimeLoginCard_Click(object sender, RoutedEventArgs e) {
        App.MainWindow.Pop(PopInfo.Info("功能开发中...", "敬请期待"));
    }

    private static bool IsLoggedIn() => BgmUserCache.Instance.User != null;

    [RelayCommand(CanExecute = nameof(IsLoggedIn))]
    private async Task OpenLinkAsync()
        => await Launcher.LaunchUriAsync(new Uri($"https://bgm.tv/user/{BgmUserCache.Instance.User!.Username}"));

    [RelayCommand(CanExecute = nameof(IsLoggedIn))]
    private async Task LogoutAsync() => await BgmUserCache.Instance.LogoutAsync();
}
