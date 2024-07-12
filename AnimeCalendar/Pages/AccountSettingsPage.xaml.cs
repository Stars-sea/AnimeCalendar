using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Auth;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Api.Storage;
using AnimeCalendar.Controls;
using AnimeCalendar.Data;
using AnimeCalendar.Storage;
using AnimeCalendar.Utils;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeCalendar.Pages;

[ObservableObject]
public sealed partial class AccountSettingsPage : Page {
    [ObservableProperty]
    private User? bgmUser;

    public AccountSettingsPage() {
        InitializeComponent();

        Loaded += (_, _) => SyncBgmCard();
    }

    public void ResetBgmCard() {
        BgmCard.Content    = "��������ϵ�¼ Bangumi ����Ȩ";
        BgmCard.IconSource = new BitmapImage(new Uri("https://bgm.tv/img/favicon.ico"));
    }

    public async void SyncBgmCard() {
        if (App.MainWindow.BgmTokenStorage == null || await App.MainWindow.BgmTokenStorage.IsExpired()) {
            ResetBgmCard();
            return;
        }

        try {
            BgmUser = await BgmApiServices.UserApi.GetMe();
        } catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("Bangumi ��Ȩ", "��ȡ�û���Ϣ����", ex));
            return;
        }

        BgmCard.IconSource = new BitmapImage(new Uri(BgmUser.Avatar.Medium));
        BgmCard.Content    = new UserInfo() { User = BgmUser };
    }

    private async Task StartBgmAuth() {
        ContentDialog dialog = new() {
            XamlRoot        = XamlRoot,
            Title           = "Bangumi ��Ȩ",
            Content         = "�㽫����ת�������, ��Ȩ��˶Ի��򽫻����йر�",
            CloseButtonText = "ȡ��",
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
            App.MainWindow.Pop(PopInfo.Info("Bangumi ��Ȩ", "������ȡ��"));
            return;
        }

        dialog.Hide();

        try {
            App.MainWindow.BgmTokenStorage = await BgmAuthTokenStorage.Request(uri.Code);

            App.MainWindow.Pop(PopInfo.Info("Bangumi ��Ȩ", "��ȡ�û���Ϣ��..."));
        }
        catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("Bangumi ��Ȩ", "�޷�ȡ�����ƻ��û���Ϣ", ex));
        }
    }

    private async void BangumiLoginCard_Click(object sender, RoutedEventArgs e) {
        if (BgmUser != null) {
            await Windows.System.Launcher.LaunchUriAsync(new Uri($"https://bgm.tv/user/{BgmUser.Username}"));
            return;
        }

        if (App.MainWindow.BgmTokenStorage == null || await App.MainWindow.BgmTokenStorage.IsExpired()) {
            await StartBgmAuth();
        }
        SyncBgmCard();
    }

    private void MikanimeLoginCard_Click(object sender, RoutedEventArgs e) {
        App.MainWindow.Pop(PopInfo.Info("���ܿ�����...", "�����ڴ�"));
    }
}
