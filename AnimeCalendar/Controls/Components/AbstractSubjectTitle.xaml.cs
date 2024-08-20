using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls.Components;

[ObservableObject]
public sealed partial class AbstractSubjectTitle : UserControl {
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SubjectRank))]
    private AbstractSubject? subject;

    [ObservableProperty]
    private bool wrap;

    [ObservableProperty]
    private bool isShowNameCn;

    public int SubjectRank => Subject switch {
        AirSubject airSubject => airSubject.Rank,
        AbstractSubject abstractSubject => abstractSubject.Rating?.Rank ?? 0,
        _ => 0
    };

    public AbstractSubjectTitle() {
        InitializeComponent();
    }

    partial void OnSubjectChanged(AbstractSubject? value) {
        RankText.Visibility  = Visibility.Collapsed;
        ScoreText.Visibility = Visibility.Collapsed;

        if (value == null || value.Rating == null) return;

        if (SubjectRank != 0)
            RankText.Visibility = Visibility.Visible;
        if (value.Rating.Score != 0)
            ScoreText.Visibility = Visibility.Visible;
    }
}
