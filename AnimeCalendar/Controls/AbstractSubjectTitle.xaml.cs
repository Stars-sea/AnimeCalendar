using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class AbstractSubjectTitle : UserControl {
    [ObservableProperty]
    private AbstractSubject? subject;

    public AbstractSubjectTitle() {
        InitializeComponent();
    }

    partial void OnSubjectChanged(AbstractSubject? value) {
        RankText.Visibility = Visibility.Collapsed;
        ScoreText.Visibility = Visibility.Collapsed;

        if (value == null || value.Rating == null) return;
        
        if (value.Rating.Rank != 0)
            RankText.Visibility = Visibility.Visible;
        if (value.Rating.Score != 0)
            ScoreText.Visibility = Visibility.Visible;
    }
}
