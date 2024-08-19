using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Api.Data;
using AnimeCalendar.Api.Mikanime;
using AnimeCalendar.Api.Mikanime.Schemas;
using AnimeCalendar.UI;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeCalendar.Pages;

using Episode = Api.Bangumi.Schemas.Episode;

[ObservableObject]
public sealed partial class SubjectDetailPage : Page {
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLoading))]
    private int runningTasksCount;
    public bool IsLoading => RunningTasksCount > 0;

    #region Bangumi
    [ObservableProperty]
    private Subject? subject;

    private IEnumerable<EpCollection> CollectedEpisode { get; set; } = [];
    #endregion

    #region Mikanime
    [ObservableProperty]
    private IEnumerable<Identifier> mikanBangumis = [];

    [ObservableProperty]
    private BangumiPage? bangumiPage;

    [ObservableProperty]
    private IEnumerable<SimpleEpisode> episodes = [];

    private IEnumerable<SimpleEpisode> FilteredEpisodes = [];

    private ObservableCollection<string> SelectedAttributes = new();
    #endregion

    public SubjectDetailPage() {
        InitializeComponent();
        SelectedAttributes.CollectionChanged += OnSelectedAttributesChanged;
    }

    #region Bangumi
    private async void UpdateCollectedEpisode(int subjectId) {
        RunningTasksCount++;
        var pagedEpCollections = await BgmApiServices.CollectionApi.GetEpisodes(subjectId, episodeType: EpType.Feature);
        CollectedEpisode = pagedEpCollections.Data;
        OnPropertyChanged(nameof(CollectedEpisode));
        RunningTasksCount--;
    }
    #endregion

    #region Mikanime
    private async void UpdateMikanBangumisAsync() {
        RunningTasksCount++;
        try {
            MikanBangumis = Subject != null
                ? await MikanimeServices.SearchAnimeApi.SearchBangumiIds(Subject.AutoName)
                : [];
        }
        catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("拉取 Mikanime 数据出错", ex));
        }
        RunningTasksCount--;
    }

    private async Task UpdateSubgroupsAsync(int mikanBangumiId) {
        RunningTasksCount++;
        try {
            BangumiPage = await MikanimeServices.BangumiApi.BangumiPage(mikanBangumiId);
        }
        catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("拉取 Mikanime 数据出错", ex));
        }
        RunningTasksCount--;
    }

    private void UpdateEpisodes(int subgroupId)
        => Episodes = BangumiPage?.GetEpisodes(subgroupId) ?? [];
    #endregion

    protected async override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);

        RunningTasksCount++;
        try {
            Subject = await BgmApiServices.SubjectApi.GetSubject((int)e.Parameter);
        }
        catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("拉取 Bangumi 数据出错", ex));
        }
        RunningTasksCount--;
    }

    #region Mikanime
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
            FilteredEpisodes = Episodes
                .Where(e => SelectedAttributes.All(a => e.Name.Contains(a)))
                .ToArray();

        OnPropertyChanged(nameof(FilteredEpisodes));
    }
    #endregion

    #region Bangumi
    partial void OnSubjectChanged(Subject? value) {
        try {
            if (value == null) return;

            UpdateCollectedEpisode(value.Id);

            RatingType rating = value.Rating;
            RatingText.Visibility = rating.Score != 0 ? Visibility.Visible : Visibility.Collapsed;
            RankText.Visibility   = rating.Rank  != 0 ? Visibility.Visible : Visibility.Collapsed;
        } finally {
            UpdateMikanBangumisAsync();
        }
    }
    #endregion

    #region Mikanime
    partial void OnMikanBangumisChanged(IEnumerable<Identifier> value) {
        BangumiSelector.ItemsSource = value;

        BangumiSelector.Visibility = value.Count() <= 1
            ? Visibility.Visible
            : Visibility.Collapsed;

        if (!value.Any()) return;

        BangumiSelector.SelectedItem = value.FirstOrDefault(
            item => string.Equals(item.Name.Trim(), Subject?.NameCn.Trim()),
            value.First()
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

    partial void OnEpisodesChanged(IEnumerable<SimpleEpisode> value) {
        SelectedAttributes.Clear();

        if (!value.Any()) {
            AttributeSelector.Visibility = Visibility.Collapsed;
            return;
        }

        AttributeSelector.Visibility = Visibility.Visible;
        AttributeSelector.SuggestedItemsSource = value.SelectMany(e => e.Attributes)
            .Distinct().Select(a => a.Trim(SimpleEpisode.Brackets));
    }
    #endregion
}
