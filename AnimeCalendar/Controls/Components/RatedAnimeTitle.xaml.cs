using AnimeCalendar.Api.Data;
using AnimeCalendar.Data;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;

using Windows.System;

namespace AnimeCalendar.Controls.Components;

[ObservableObject]
public sealed partial class RatedAnimeTitle : UserControl {
    [ObservableProperty]
    private IRatedAnime? anime;

    [ObservableProperty]
    private bool isWrap;

    [ObservableProperty]
    private bool isShowAutoName;

    [ObservableProperty]
    private bool isShowHyperlink;

    public RatedAnimeTitle() {
        InitializeComponent();
    }

    private async void HyperlinkButton_Click(object sender, RoutedEventArgs e) {
        if (Anime == null || Anime.Website != Website.Bangumi) return;
        try {
            await Launcher.LaunchUriAsync(new($"https://bgm.tv/subject/{Anime!.Id}"));
        } catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail(ex));
        }
    }

    partial void OnAnimeChanged(IRatedAnime? value) {
        RankText.Visibility = value == null || value.Rank <= 0
            ? Visibility.Collapsed
            : Visibility.Visible;

        ScoreText.Visibility = value == null || value.Score <= 0
            ? Visibility.Collapsed
            : Visibility.Visible;
    }
}
