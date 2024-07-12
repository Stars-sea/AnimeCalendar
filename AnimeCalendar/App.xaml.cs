using System;
using System.Diagnostics;
using System.Threading.Channels;

using AnimeCalendar.Data;

using H.NotifyIcon;

using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

using Windows.ApplicationModel.Activation;

namespace AnimeCalendar;

public partial class App : Application {

    public App() {
        InitializeComponent();
    }

    protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
        AppInstance instance = AppInstance.FindOrRegisterForKey("AnimeCalendar.Main");

        if (instance.IsCurrent) {
            mainWindow = new MainWindow();
            MainWindow.Activate();

            instance.Activated += Instance_Activated;
            return;
        }

        var activatedArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
        await instance.RedirectActivationToAsync(activatedArgs);
        Process.GetCurrentProcess().Kill();
    }

    private async void Instance_Activated(object? sender, AppActivationArguments e) {
        MainWindow.Show();
        if (e.Data is not ProtocolActivatedEventArgs args) return;

        Uri uri = args.Uri;
        Trace.TraceInformation($"Received activation uri: {uri}");

        try {
            await CallbackUriChannel.Writer.WriteAsync(uri);
        } catch (Exception ex) {
            Trace.TraceWarning(ex.ToString());
            MainWindow.Pop(PopInfo.Warn("Warning", ex.ToString()));
        }
    }

    private static MainWindow? mainWindow;
    private static readonly Channel<CallbackUri> CallbackUriChannel = Channel.CreateBounded<CallbackUri>(3);

    internal static MainWindow MainWindow => mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
    internal static ChannelReader<CallbackUri> CallbackUri => CallbackUriChannel.Reader;
}
