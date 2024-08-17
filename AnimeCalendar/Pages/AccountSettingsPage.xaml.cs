using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Auth;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Api.Storage;
using AnimeCalendar.Controls;
using AnimeCalendar.Data;
using AnimeCalendar.UI;
using AnimeCalendar.Storage;
using AnimeCalendar.Utils;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeCalendar.Pages;

[ObservableObject]
public sealed partial class AccountSettingsPage : Page {
    [ObservableProperty]
    private User? bgmUser;

    public AccountSettingsPage() {
        InitializeComponent();

        Loaded   += AccountSettingsPage_Loaded;
        Unloaded += AccountSettingsPage_Unloaded;
    }

    partial void OnBgmUserChanged(User? value) {
        if (value == null) {
            BgmCard.Content    = new TextBlock { Text = "在浏览器上登录 Bangumi 并授权", VerticalAlignment = VerticalAlignment.Center };
            BgmCard.IconSource = new BitmapImage(new Uri("https://bgm.tv/img/favicon.ico"));
        }
        else {
            BgmCard.Content    = new BgmUserInfo() { User = value };
            BgmCard.IconSource = new BitmapImage(value.Avatar.Medium);
        }
    }

    public async void FetchBgmUser(IAuthTokenStorage? token) {
        if (token == null || await token.IsExpired()) {
            BgmUser = null;
            return;
        }

        try {
            BgmUser = await BgmApiServices.UserApi.GetMe();
        } catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("Bangumi 登录", "拉取用户信息错误", ex));
            BgmUser = null;
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

    private void AccountSettingsPage_Loaded(object sender, RoutedEventArgs e) {
        FetchBgmUser(App.MainWindow.BgmTokenStorage);
        App.MainWindow.BgmTokenChanged += FetchBgmUser;
    }

    private void AccountSettingsPage_Unloaded(object sender, RoutedEventArgs e) {
        App.MainWindow.BgmTokenChanged -= FetchBgmUser;
    }

    private async void BangumiLoginCard_Click(object sender, RoutedEventArgs e) {
        if (App.MainWindow.BgmTokenStorage == null || await App.MainWindow.BgmTokenStorage.IsExpired()) {
            if (await StartBgmAuth() is IAuthTokenStorage authToken)
                App.MainWindow.BgmTokenStorage = authToken;
        } else if (BgmUser != null) {
            await Windows.System.Launcher.LaunchUriAsync(new Uri($"https://bgm.tv/user/{BgmUser.Username}"));
        }
    }

    private void MikanimeLoginCard_Click(object sender, RoutedEventArgs e) {
        App.MainWindow.Pop(PopInfo.Info("功能开发中...", "敬请期待"));
    }
}
