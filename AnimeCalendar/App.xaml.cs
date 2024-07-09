﻿using System;
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
            MainWindow = new MainWindow();
            MainWindow.Activate();

            instance.Activated += Instance_Activated;
            return;
        }

        var activatedArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
        await instance.RedirectActivationToAsync(activatedArgs);
        Process.GetCurrentProcess().Kill();
    }

    private async void Instance_Activated(object sender, AppActivationArguments e) {
        MainWindow.Show();
        var args = e.Data as ProtocolActivatedEventArgs;
        if (args == null) return;

        Uri uri = args.Uri;
        Debug.WriteLine(uri);

        try {
            await CallbackUriChannel.Writer.WriteAsync(args.Uri);
        } catch (Exception ex) {
            Trace.TraceWarning(ex.ToString());
            MainWindow.PostInfo(GlobalInfo.Warning("Warning", ex.ToString()));
        }
    }

    private static readonly Channel<CallbackUri> CallbackUriChannel = Channel.CreateBounded<CallbackUri>(3);

    public static MainWindow MainWindow { get; private set; }

    internal static ChannelReader<CallbackUri> CallbackUri => CallbackUriChannel.Reader;
}
