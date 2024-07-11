using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Storage;
using AnimeCalendar.Data;
using AnimeCalendar.Storage;

using CommunityToolkit.Mvvm.Input;

using H.NotifyIcon;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System.Diagnostics;
using System.Threading.Tasks;

namespace AnimeCalendar;

public sealed partial class MainWindow : Window {
    private static IAuthTokenStorage? _AuthTokenStorage;
    internal static IAuthTokenStorage? BgmTokenStorage { 
        get => _AuthTokenStorage;
        set {
            _AuthTokenStorage = value;
            BgmApiServices.UpdateTokenStorage(value);
        }
    }

    public MainWindow() {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);

        Activated   += MainWindow_Activated;
        Closed      += MainWindow_Closed;
    }

    public async void PostInfo(GlobalInfo info) {
        Trace.WriteLine(info);

        InfoBar infoBar = new() {
            Title       = info.Title,
            Message     = info.Message,
            Severity    = info.Severity,
            IsOpen      = true,
        };

        InfoQueue.Children.Add(infoBar);

        await Task.Delay(info.Duration);
        InfoQueue.Children.Remove(infoBar);
    }

    private async void MainWindow_Activated(object sender, WindowActivatedEventArgs args) {
        BgmTokenStorage = await BgmAuthTokenStorage.Load();
        try {
            if (BgmTokenStorage != null)
                await BgmTokenStorage.RefreshIfExpired();
        } catch {
            GlobalInfo warn = GlobalInfo.Warning("Bangumi ÁîÅÆË¢ÐÂ", "Ë¢ÐÂÊ§°Ü");
            PostInfo(warn);
        }
    }

    private void MainWindow_Closed(object sender, WindowEventArgs args) {
        args.Handled = true;
        this.Hide();
    }

    [RelayCommand]
    public void ShowHideWindow() {
        if (Visible)
            this.Hide();
        else this.Show();
       
    }

    [RelayCommand]
    public void ShowWindow() {
        if (!Visible) this.Show();
    }

    [RelayCommand]
    public void ExitApplication() {
        Closed -= MainWindow_Closed;

        TrayIcon.Dispose();
        Close();
    }
}
