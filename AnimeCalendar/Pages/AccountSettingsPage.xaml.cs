using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Auth;
using AnimeCalendar.Api.Storage;
using AnimeCalendar.Data;
using AnimeCalendar.Storage;
using AnimeCalendar.Utils;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Threading.Tasks;

using Windows.System;

namespace AnimeCalendar.Pages;

public sealed partial class AccountSettingsPage : Page {
    public AccountSettingsPage() {
        InitializeComponent();
    }

    private async void BangumiLoginCard_Click(object sender, RoutedEventArgs e) {
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
        await Launcher.LaunchUriAsync(new Uri(IAuthApi.AuthorizePageUrl));

        object result = await Task.WhenAny(showDialog.WrapTask(), readCallback.WrapTask());
        if (result is CallbackUri uri) {
            IAuthTokenStorage tokenStorage = await BgmAuthTokenStorage.Request(uri.Code);
            BangumiApiServices.Init(tokenStorage);
            // TODO...
        }
    }

    private void MikanimeLoginCard_Click(object sender, RoutedEventArgs e) {
        App.MainWindow.PostInfo(GlobalInfo.Information("���ܿ�����...", "�����ڴ�"));
    }
}
