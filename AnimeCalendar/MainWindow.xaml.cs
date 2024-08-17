using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Storage;
using AnimeCalendar.Storage;
using AnimeCalendar.UI;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using H.NotifyIcon;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AnimeCalendar;

[ObservableObject]
public sealed partial class MainWindow : Window {
    [ObservableProperty]
    private IAuthTokenStorage? bgmTokenStorage;

    internal event Action<IAuthTokenStorage?>? BgmTokenChanged;

    public MainWindow() {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);

        Activated   += MainWindow_Activated;
        Closed      += MainWindow_Closed;
    }

    internal async void Pop(PopInfo info) {
        Trace.WriteLine(info);

        InfoBar infoBar = new() {
            Title       = info.Title,
            Message     = info.Message,
            Severity    = info.Severity,
            IsOpen      = true,
            TranslationTransition = new Vector3Transition()
        };

        InfoQueue.Children.Add(infoBar);

        if (info.Duration != System.TimeSpan.Zero) {
            await Task.Delay(info.Duration);
            infoBar.Translation = new System.Numerics.Vector3(450, 0 , 0);
            await Task.Delay(300);
            InfoQueue.Children.Remove(infoBar);
        }
    }

    partial void OnBgmTokenStorageChanged(IAuthTokenStorage? value) {
        BgmApiServices.UpdateTokenStorage(value);
        BgmTokenChanged?.Invoke(value);
    }

    private async void MainWindow_Activated(object sender, WindowActivatedEventArgs args) {
        BgmTokenStorage = await BgmAuthTokenStorage.Load();
        try {
            if (BgmTokenStorage != null)
                await BgmTokenStorage.RefreshIfExpired();
        } catch {
            Pop(PopInfo.Warn("Bangumi  ⁄»®", "¡Ó≈∆À¢–¬ ß∞‹"));
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
