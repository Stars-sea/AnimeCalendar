using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using System.Collections.Generic;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class WatchProgressControl : UserControl {
    [ObservableProperty]
    private IEnumerable<EpCollection> episodes = [];

    public WatchProgressControl() {
        InitializeComponent();
    }
}