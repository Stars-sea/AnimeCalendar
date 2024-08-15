using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Api.Data;
using AnimeCalendar.Api.Mikanime;
using AnimeCalendar.Api.Mikanime.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;

using Newtonsoft.Json.Linq;

using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.System;

namespace AnimeCalendar.Pages;

[ObservableObject]
public sealed partial class SubjectDetialPage : Page {
    [ObservableProperty]
    private Subject? subject;

    public string? SubjectName => Subject != null
        ? string.IsNullOrEmpty(Subject.NameCn) ? Subject.Name : Subject.NameCn
        : null;

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private Identifier[] mikanBangumis = [];

    [ObservableProperty]
    private BangumiPage? bangumiPage;

    [ObservableProperty]
    private SimpleEpisode[] episodes = [];

    public SubjectDetialPage() {
        InitializeComponent();
    }

    private async void UpdateMikanBangumisAsync() {
        IsLoading = true;
        MikanBangumis = Subject != null
            ? await MikanimeServices.SearchAnimeApi.SearchBangumiIds(Subject.NameCn)
            : [];
        IsLoading = false;
    }

    private async Task UpdateSubgroupsAsync(int mikanBangumiId) {
        IsLoading = true;
        BangumiPage = await MikanimeServices.BangumiApi.BangumiPage(mikanBangumiId);
        IsLoading = false;
    }

    private void UpdateEpisodes(int subgroupId)
        => Episodes = BangumiPage?.GetEpisodes(subgroupId) ?? [];

    protected async override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);

        int subjectId = (int)e.Parameter;
        Subject = await BgmApiServices.SubjectApi.GetSubject(subjectId);
    }

    private async void OnSelectedBangumiChanged(object sender, SelectionChangedEventArgs e) {
        if (BangumiSelector.SelectedItem is Identifier identifier)
            await UpdateSubgroupsAsync(identifier.Id);
    }

    private void OnSelectedSubgroupChanged(object sender, SelectionChangedEventArgs e) {
        if (SubgroupSelector.SelectedItem is Identifier identifier)
            UpdateEpisodes(identifier.Id);
    }

    private async void OnEpisodeItemDoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {
        var episode = (SimpleEpisode)((ItemContainer)sender).Tag;
        await Launcher.LaunchUriAsync(new Uri(episode.Link));
    }

    partial void OnSubjectChanged(Subject? value) {
        OnPropertyChanged(nameof(SubjectName));

        try {
            if (value == null) return;

            RatingType rating = value.Rating;
            RatingText.Visibility = rating.Score != 0 ? Visibility.Visible : Visibility.Collapsed;
            RankText.Visibility   = rating.Rank  != 0 ? Visibility.Visible : Visibility.Collapsed;
        } finally {
            UpdateMikanBangumisAsync();
        }
    }

    partial void OnMikanBangumisChanged(Identifier[] value) {
        BangumiSelector.ItemsSource = value;

        BangumiSelector.Visibility = value.Length <= 1
            ? Visibility.Collapsed
            : Visibility.Visible;

        if (value.Length == 0) return;

        BangumiSelector.SelectedItem = value.FirstOrDefault(
            item => string.Equals(item.Name.Trim(), Subject?.NameCn.Trim()),
            value[0]
        );
    }

    partial void OnBangumiPageChanged(BangumiPage? value) {
        Identifier[] subgroups = value?.Subgroups ?? [];

        SubgroupSelector.ItemsSource = subgroups;
        SubgroupSelector.SelectedItem = subgroups.FirstOrDefault();

        SubgroupSelector.Visibility = subgroups.Length == 0
            ? Visibility.Collapsed
            : Visibility.Visible;
    }
}
