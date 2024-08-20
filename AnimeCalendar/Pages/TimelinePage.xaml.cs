using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.UI;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeCalendar.Pages;

public sealed partial class TimelinePage : Page {
    private static Calendar[]? cache;

    public Calendar? Calendar { get; private set; }
    public int       Weekday  { get; private set; }

    public TimelinePage() {
        InitializeComponent();
        Loaded += async (_, _) => await Reload();
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

    public async Task Reload() {
        LoadingRing.Visibility = Visibility.Visible;
        SubjectList.Visibility = Visibility.Collapsed;

        Calendar = await UpdateCalendar(Weekday);
        if (Calendar == null) return;

        LoadingRing.Visibility = Visibility.Collapsed;
        SubjectList.Visibility = Visibility.Visible;

        // TODO: Filter
        SubjectList.ItemsSource = Calendar.Items
            .OrderBy(i => i.Rank == 0 ? int.MaxValue : i.Rank)
            .ThenByDescending(i => i.Rating?.Score);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        Weekday = Convert.ToInt32(e.Parameter);
    }

    private void SubjectList_SelectionChanged(ItemsView sender, ItemsViewSelectionChangedEventArgs args) {
        if (sender.SelectedItem is not AirSubject tag) return;

        NavigationInfo navigationInfo = new(
            typeof(SubjectDetailPage), null, tag.Id, $"{nameof(TimelinePage)}#{tag.AirWeekday}"
        );
        IndexPage.Current?.Navigate(navigationInfo);
    }
}
