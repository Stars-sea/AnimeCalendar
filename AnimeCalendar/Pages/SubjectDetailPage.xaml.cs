using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
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

    [ObservableProperty]
    private Subject? subject;

    public SubjectDetailPage() {
        InitializeComponent();
    }

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
}
