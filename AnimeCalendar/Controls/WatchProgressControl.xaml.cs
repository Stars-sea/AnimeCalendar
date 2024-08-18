using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System.Collections.Generic;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class WatchProgressControl : UserControl {
    public int RunningTasksCount {
        get { return (int)GetValue(RunningTasksCountProperty); }
        set { SetValue(RunningTasksCountProperty, value); }
    }

    public static readonly DependencyProperty RunningTasksCountProperty = 
        DependencyProperty.Register("RunningTasksCount", typeof(int),
            typeof(WatchProgressControl), new PropertyMetadata(0));

    [ObservableProperty]
    private IEnumerable<EpCollection> episodes = [];

    public WatchProgressControl() {
        InitializeComponent();
    }

    private void OnSyncing(object sender, RoutedEventArgs e) => RunningTasksCount++;
    private void OnSynced(object sender, RoutedEventArgs e) => RunningTasksCount--;
}