using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class AirSubjectInfo : UserControl {
    [ObservableProperty]
    private AirSubject? subject;

    public AirSubjectInfo() {
        InitializeComponent();
    }

    partial void OnSubjectChanged(AirSubject? value) {
        RankText.Visibility = Visibility.Collapsed;
        ScoreText.Visibility = Visibility.Collapsed;

        if (value == null) return;
        
        if (value.Rank != 0)
            RankText.Visibility = Visibility.Visible;
        if (value.Rating != null && value.Rating.Score != 0)
            ScoreText.Visibility = Visibility.Visible;
    }
}
