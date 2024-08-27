using AnimeCalendar.Api.Data;
using AnimeCalendar.Api.Mikanime;
using AnimeCalendar.Api.Mikanime.Schemas;
using AnimeCalendar.Controls.Base;
using AnimeCalendar.Data;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class MikanimeView : TasksCountableControl {
    #region Fields & Props
    [ObservableProperty]
    private string? subjectName;

    [ObservableProperty]
    private IEnumerable<Identifier> mikanBangumis = [];

    [ObservableProperty]
    private BangumiPage? bangumiPage;

    [ObservableProperty]
    private IEnumerable<SimpleEpisode> episodes = [];

    private IEnumerable<SimpleEpisode> FilteredEpisodes = [];

    private ObservableCollection<string> SelectedAttributes = new();
    #endregion

    public MikanimeView() {
        InitializeComponent();
        SelectedAttributes.CollectionChanged += OnSelectedAttributesChanged;
    }

    #region Updaters
    private async Task UpdateMikanBangumisAsync() {
        RunningTasksCount++;
        try {
            MikanBangumis = SubjectName != null
                ? await MikanimeServices.SearchAnimeApi.SearchBangumiIds(SubjectName)
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

    #region Event Handlers
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
                .Where(e => SelectedAttributes.All(a => e.Name.Contains(a)));

        OnPropertyChanged(nameof(FilteredEpisodes));
    }
    #endregion

    #region Listeners
    partial void OnSubjectNameChanged(string? value)
        => UpdateMikanBangumisAsync().ConfigureAwait(false);

    partial void OnMikanBangumisChanged(IEnumerable<Identifier> value) {
        BangumiSelector.ItemsSource = value;

        BangumiSelector.Visibility = value.Count() <= 1
            ? Visibility.Collapsed
            : Visibility.Visible;

        if (!value.Any()) return;

        BangumiSelector.SelectedItem = value.FirstOrDefault(
            item => string.Equals(item.Name.Trim(), SubjectName),
            value.First()
        );
    }

    partial void OnBangumiPageChanged(BangumiPage? value) {
        Identifier[] subgroups = value?.Subgroups ?? [];

        SubgroupSelector.ItemsSource  = subgroups;
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