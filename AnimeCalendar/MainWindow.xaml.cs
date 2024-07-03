using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using H.NotifyIcon.Core;
using H.NotifyIcon;

using Microsoft.UI.Xaml;

namespace AnimeCalendar;

public sealed partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);

        Closed += MainWindow_Closed;
    }

    private void MainWindow_Closed(object sender, WindowEventArgs args) {
        args.Handled = true;
        this.Hide();
    }

    [RelayCommand]
    public void ShowHideWindow() {
        if (Visible) {
            this.Hide();
        }
        else {
            this.Show();
        }
    }

    [RelayCommand]
    public void ShowWindow() {
        if (!Visible) {
            this.Show();
        }
    }

    [RelayCommand]
    public void ExitApplication() {
        Closed -= MainWindow_Closed;

        TrayIcon.Dispose();
        Close();
    }
}
