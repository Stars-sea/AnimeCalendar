using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;

namespace AnimeCalendar.Pages;

[ObservableObject]
public sealed partial class SubjectDetialPage : Page {
    [ObservableProperty]
    private Subject? subject;

    public SubjectDetialPage() {
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);

        int subjectId = (int)e.Parameter;
        Subject = await BgmApiServices.SubjectApi.GetSubject(subjectId);
    }

    partial void OnSubjectChanged(Subject? value) {
        if (value == null) return;

        RatingType rating = value.Rating;
        RatingText.Visibility = rating.Score != 0 ? Visibility.Visible : Visibility.Collapsed;
        RankText.Visibility   = rating.Rank  != 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    private void Image_PointerPressed(object sender, PointerRoutedEventArgs e) {
        // TODO: œ‘ æ¥ÛÕº
    }
}
