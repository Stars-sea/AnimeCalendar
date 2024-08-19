using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Controls.Base;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System.Collections.Generic;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class WatchProgressControl : TasksCountableControl {
    [ObservableProperty]
    private int subjectId;

    private IEnumerable<EpCollection> Episodes { get; set; } = [];

    public WatchProgressControl() {
        InitializeComponent();
    }

    private void OnSyncing(object sender, RoutedEventArgs e) => RunningTasksCount++;
    private void OnSynced(object sender, RoutedEventArgs e) => RunningTasksCount--;

    private async void UpdateCollectedEpisode() {
        RunningTasksCount++;
        var collectedEps = await BgmApiServices.CollectionApi.GetEpisodes(SubjectId, episodeType: EpType.Feature);
        Episodes = collectedEps.Data;
        OnPropertyChanged(nameof(Episodes));
        RunningTasksCount--;
    }

    partial void OnSubjectIdChanged(int value) => UpdateCollectedEpisode();
}