using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Data;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeCalendar.Pages;

public sealed partial class TimelinePage : Page {
    private static Calendar[]? cache;
    private static bool IsShowFiltered = false;

    public Calendar? Calendar { get; private set; }
    public int       Weekday  { get; private set; }

    private string WeekdayCn => "周" + "一二三四五六日"[Weekday - 1];

    public TimelinePage() {
        InitializeComponent();
        Loaded += async (_, _) => await Reload(IsShowFiltered);
    }

    private static async Task<Calendar?> UpdateCalendar(int weekday) {
        try {
            if (cache == null) {
                cache = await BgmApiServices.SubjectApi.GetCalendar();
            }
            return cache.First(c => c.Weekday.Id == weekday.ToString());
        }
        catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("新番时间表", "拉取数据失败", ex));
        }
        return null;
    }

    public async Task Reload(bool isShowFiltered = false) {
        LoadingRing.Visibility = Visibility.Visible;
        SubjectList.Visibility = Visibility.Collapsed;

        Calendar = await UpdateCalendar(Weekday);
        if (Calendar == null) return;

        var items = Calendar.Items
            .OrderBy(i => i.Rank == 0 ? int.MaxValue : i.Rank)
            .ThenByDescending(i => i.Rating?.Score);

        List<AirSubject> subjects = new();
        foreach (AirSubject subject in items) {
            if (!isShowFiltered || (isShowFiltered && await IsCollected(subject.Id)))
                subjects.Add(subject);
        }

        LoadingRing.Visibility = Visibility.Collapsed;
        SubjectList.Visibility = Visibility.Visible;
        
        SubjectList.ItemsSource = subjects;
    }

    private static async ValueTask<bool> IsCollected(int subjectId) {
        User? user = BgmUserCache.Instance.User;
        if (user == null) return true;

        try {
            await BgmApiServices.CollectionApi.GetCollection(user.Username, subjectId);
            return true;
        } catch {
            return false;
        }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        Weekday = Convert.ToInt32(e.Parameter);
    }

    private void SubjectList_SelectionChanged(ItemsView sender, ItemsViewSelectionChangedEventArgs args) {
        if (sender.SelectedItem is not AirSubject tag) return;

        NavigationInfo navigationInfo = new(
            typeof(SubjectDetailPage), tag.Id, $"{nameof(TimelinePage)}#{tag.AirWeekday}"
        );
        IndexPage.Current?.Navigate(navigationInfo);
    }

    private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e) {
        var toggleSwitch = (ToggleSwitch)sender;
        await Reload(IsShowFiltered = toggleSwitch.IsOn);
    }
}
