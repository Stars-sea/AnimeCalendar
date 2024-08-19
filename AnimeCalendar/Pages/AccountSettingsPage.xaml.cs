using AnimeCalendar.Api.Bangumi.Auth;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Api.Storage;
using AnimeCalendar.Controls;
using AnimeCalendar.Data;
using AnimeCalendar.UI;
using AnimeCalendar.Storage;
using AnimeCalendar.Utils;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeCalendar.Pages;

public sealed partial class AccountSettingsPage : Page {
    public AccountSettingsPage() {
        InitializeComponent();

        Loaded   += AccountSettingsPage_Loaded;
        Unloaded += AccountSettingsPage_Unloaded;
    }

    private void OnBgmUserChanged(User? value) {
        if (value == null) {
            BgmCard.Content    = new TextBlock { Text = "��������ϵ�¼ Bangumi ����Ȩ", VerticalAlignment = VerticalAlignment.Center };
            BgmCard.IconSource = new BitmapImage(new Uri("https://bgm.tv/img/favicon.ico"));
        }
        else {
            BgmCard.Content    = new BgmUserInfo() { User = value };
            BgmCard.IconSource = new BitmapImage(value.Avatar.Medium);
        }
    }

    private async Task<IAuthTokenStorage?> StartBgmAuth() {
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
            return null;
        }

        dialog.Hide();

        try {
            App.MainWindow.Pop(PopInfo.Info("Bangumi ��Ȩ", "��ȡ������..."));
            return await BgmAuthTokenStorage.Request(uri.Code);
        }
        catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("Bangumi ��Ȩ", "�޷�ȡ������", ex));
        }
        return null;
    }

    private void AccountSettingsPage_Loaded(object sender, RoutedEventArgs e) {
        OnBgmUserChanged(BgmUserCache.Instance.User);

        BgmUserCache.UserChanged += OnBgmUserChanged;
        BgmUserCache.Instance.UpdateUserAsync();
    }

    private void AccountSettingsPage_Unloaded(object sender, RoutedEventArgs e) {
        BgmUserCache.UserChanged -= OnBgmUserChanged;
    }

    private async void BangumiLoginCard_Click(object sender, RoutedEventArgs e) {
        BgmUserCache cache = BgmUserCache.Instance;

        if (await BgmUserCache.IsTokenNullOrExpired()) {
            if (await StartBgmAuth() is IAuthTokenStorage authToken)
                cache.TokenStorage = authToken;
        } else if (cache.User != null) {
            await Windows.System.Launcher.LaunchUriAsync(new Uri($"https://bgm.tv/user/{cache.User.Username}"));
        }
    }

    private void MikanimeLoginCard_Click(object sender, RoutedEventArgs e) {
        App.MainWindow.Pop(PopInfo.Info("���ܿ�����...", "�����ڴ�"));
    }
}
