using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Auth;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Controls;
using AnimeCalendar.Data;
using AnimeCalendar.Storage;
using AnimeCalendar.Utils;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

using System;
using System.Threading.Tasks;

namespace AnimeCalendar.Pages;

[ObservableObject]
public sealed partial class AccountSettingsPage : Page {
    [ObservableProperty]
    private User? bgmUser;

    public AccountSettingsPage() {
        InitializeComponent();

        Loaded += AccountSettingsPage_Loaded;
    }

    public void ResetBgmCard() {
        BgmCard.Content    = "��������ϵ�¼ Bangumi ����Ȩ";
        BgmCard.IconSource = new BitmapImage(new Uri("https://bgm.tv/img/favicon.ico"));
    }

    public async Task<bool> UpdateBgmUserInfo() {
        if (MainWindow.BgmTokenStorage == null ||
            await MainWindow.BgmTokenStorage.IsExpired())
            return false;

        try {
            BgmUser = await BgmApiServices.UserApi.GetMe();
        }
        catch (Exception ex) {
            var error = GlobalInfo.Error("Bangumi ��Ȩ", "��ȡ�û���Ϣ����", ex);
            App.MainWindow.PostInfo(error);
            return false;
        }
        return true;
    }

    public void UpdateBgmCard() {
        if (BgmUser == null) return;

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
        var showDialog   = dialog.ShowAsync();
        var readCallback = App.CallbackUri.ReadAsync();

        await Task.Delay(800);
        await Windows.System.Launcher.LaunchUriAsync(new Uri(AuthService.AUTH_PAGE_URL));

        Task<object?> result = await Task.WhenAny(showDialog.WrapTask(), readCallback.WrapTask());
        if (await result is not CallbackUri uri) return;

        dialog.Hide();

        try {
            var tokenStorage = await BgmAuthTokenStorage.Request(uri.Code);
            MainWindow.BgmTokenStorage = tokenStorage;

            App.MainWindow.PostInfo(GlobalInfo.Information("Bangumi ��Ȩ", "��ȡ�û���Ϣ��..."));
        }
        catch (Exception ex) {
            GlobalInfo err = GlobalInfo.Error("Bangumi ��Ȩ", "�޷�ȡ�����ƻ��û���Ϣ", ex);
            App.MainWindow.PostInfo(err);
        }
    }

    private async void AccountSettingsPage_Loaded(object sender, RoutedEventArgs e) {
        if (await UpdateBgmUserInfo())
            UpdateBgmCard();
        else ResetBgmCard();
    }

    private async void BangumiLoginCard_Click(object sender, RoutedEventArgs e) {
        if (MainWindow.BgmTokenStorage == null ||
            await MainWindow.BgmTokenStorage.IsExpired()) {
            await StartBgmAuth();
            await UpdateBgmUserInfo();
            UpdateBgmCard();
            return;
        }

        await Windows.System.Launcher.LaunchUriAsync(new Uri($"https://bgm.tv/user/{BgmUser!.Username}"));
    }

    private void MikanimeLoginCard_Click(object sender, RoutedEventArgs e) {
        App.MainWindow.PostInfo(GlobalInfo.Information("���ܿ�����...", "�����ڴ�"));
    }
}
