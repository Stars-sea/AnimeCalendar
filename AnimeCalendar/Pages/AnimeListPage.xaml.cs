using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Data;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeCalendar.Pages;

public sealed partial class AnimeListPage : Page {
    private static Calendar[]? cache;

    public Calendar? Calendar { get; private set; }
    public int       Weekday  { get; private set; }

    public AnimeListPage() {
        InitializeComponent();
        Loaded += async (_, _) => await Reload();
    }

    private static async Task<Calendar?> UpdateCalendar(int weekday) {
        try {
            if (cache == null) {
                cache = await BgmApiServices.SubjectApi.GetCalendar();
            }
            return cache.First(c => c.Weekday.Id == weekday.ToString());
        } catch (Exception ex) {
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

    private void SubjectList_ItemInvoked(ItemsView sender, ItemsViewItemInvokedEventArgs args) {
        AirSubject tag = (AirSubject)args.InvokedItem;

        NavigationInfo navigationInfo = new(
            typeof(SubjectDetialPage), null, tag.Id
        );
        IndexPage.Current?.Navigate(navigationInfo);
    }
}
