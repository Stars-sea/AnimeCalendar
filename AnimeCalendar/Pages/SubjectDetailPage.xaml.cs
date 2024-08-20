using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Data;
using AnimeCalendar.UI;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;

namespace AnimeCalendar.Pages;

[ObservableObject]
public sealed partial class SubjectDetailPage : Page {
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLoading))]
    private int runningTasksCount;
    public bool IsLoading => RunningTasksCount > 0;

    #region Fields & Props
    [ObservableProperty]
    private Subject? subject;

    private string CollectionStatus => SubjectCollection?.Type switch {
        CollectionType.Do       => "在看",
        CollectionType.Wish     => "想看",
        CollectionType.Collect  => "看过",
        CollectionType.OnHold   => "搁置",
        CollectionType.Dropped  => "抛弃",
        _ => ""
    };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CollectionStatus))]
    private UserCollection? subjectCollection;
    #endregion

    public SubjectDetailPage() {
        InitializeComponent();
    }

    #region Updaters
    private async void UpdateSubjectCollection(int subjectId) {
        if (BgmUserCache.Instance.User == null)
            return;
        string username = BgmUserCache.Instance.User.Username;

        RunningTasksCount++;
        try {
            SubjectCollection = await BgmApiServices.CollectionApi.GetCollection(username, subjectId);
        }
        catch {
            SubjectCollection = null;
        }
        RunningTasksCount--;
    }
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

    #region Listeners
    partial void OnSubjectChanged(Subject? value) {
        if (value == null) return;
        UpdateSubjectCollection(value.Id);
    }
    #endregion
}
