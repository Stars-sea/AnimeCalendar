using AnimeCalendar.Api.Mikanime.Schemas;
using AnimeCalendar.Data;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Controls;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.System;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class EpisodeInfoCard : ItemContainer {
    [ObservableProperty]
    private SimpleEpisode episode;

    [ObservableProperty]
    private bool isShowPureName = true;

    private MetadataItem[] Metadatas { get; set; } = [];

    public EpisodeInfoCard() {
        InitializeComponent();
        DoubleTapped += OnDoubleTapped;
    }

    private async void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {
        App.MainWindow.Pop(PopInfo.Info("", "正在打开磁力链接"));
        try {
            await Launcher.LaunchUriAsync(new Uri(Episode.Magnet));
        }
        catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("打开磁链错误", "尝试复制磁链到剪贴板", ex));
            CopyMagnet();
        }
    }

    partial void OnEpisodeChanged(SimpleEpisode value) {
        // TODO
        //IsShowPureName = value.BangumiName == null ||
        //    !SimpleEpisode.Brackets.SkipLast(1).Any(c => value.BangumiName.Contains(c));

        Metadatas = value.Attributes.Select(a => new MetadataItem {
            Label = a
        }).ToArray();
        OnPropertyChanged(nameof(Metadatas));
    }

    [RelayCommand]
    private void CopyMagnet() {
        DataPackage package = new();
        package.SetText(Episode.Magnet);
        Clipboard.SetContent(package);
        App.MainWindow.Pop(PopInfo.Succ("", "已复制磁力链接"));
    }

    [RelayCommand]
    private async Task OpenLinkInBrowser() {
        App.MainWindow.Pop(PopInfo.Info("", "正在从浏览器打开链接"));
        await Launcher.LaunchUriAsync(new Uri(Episode.Link));
    }
}