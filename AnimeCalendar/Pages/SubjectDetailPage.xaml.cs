using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Api.Data;
using AnimeCalendar.Api.Mikanime;
using AnimeCalendar.Api.Mikanime.Schemas;
using AnimeCalendar.Controls;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeCalendar.Pages;

[ObservableObject]
public sealed partial class SubjectDetialPage : Page {
    [ObservableProperty]
    private Subject? subject;

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private Identifier[] mikanBangumis = [];

    [ObservableProperty]
    private BangumiPage? bangumiPage;

    [ObservableProperty]
    private SimpleEpisode[] episodes = [];

    private SimpleEpisode[] FilteredEpisodes = [];

    private ObservableCollection<string> SelectedAttributes = new();

    public SubjectDetialPage() {
        InitializeComponent();
        SelectedAttributes.CollectionChanged += OnSelectedAttributesChanged;
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

    private void OnSelectedAttributesChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        if (SelectedAttributes.Count == 0)
            FilteredEpisodes = Episodes;
        else
            FilteredEpisodes = Episodes.Where(e => {
                var epAttri = e.Attributes.Select(a => a.Trim(SimpleEpisode.Brackets));
                return SelectedAttributes.All(a => epAttri.Any(epA => epA.Contains(a)));
            }).ToArray();

        OnPropertyChanged(nameof(FilteredEpisodes));
    }

    partial void OnSubjectChanged(Subject? value) {
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

    partial void OnEpisodesChanged(SimpleEpisode[] value) {
        SelectedAttributes.Clear();

        if (value.Length == 0) {
            AttributeSelector.Visibility = Visibility.Collapsed;
            return;
        }

        AttributeSelector.Visibility = Visibility.Visible;
        AttributeSelector.SuggestedItemsSource = value.SelectMany(e => e.Attributes)
            .Distinct().Select(a => a.Trim(SimpleEpisode.Brackets));
    }
}
