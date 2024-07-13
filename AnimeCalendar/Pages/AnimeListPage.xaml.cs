using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Controls;
using AnimeCalendar.Data;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.System;

namespace AnimeCalendar.Pages;

public sealed partial class AnimeListPage : Page {
    internal static Cache<Calendar[]> CalendarCache = new(null, UpdateCalendar, TimeSpan.FromHours(1));
    
    public Calendar? Calendar { get; private set; }
    public int       Weekday  { get; private set; }

    public AnimeListPage() {
        InitializeComponent();

        Loaded  += async (_, _) => await Reload();
    }

    private static async Task<Calendar[]?> UpdateCalendar() {
        try {
            return await BgmApiServices.SubjectApi.GetCalendar();
        } catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("新番时间表", "拉取数据失败", ex));
        }
        return null;
    }

    public async Task Reload() {
        LoadingRing.Visibility = Visibility.Visible;
        SubjectList.Visibility = Visibility.Collapsed;

        Calendar = (await CalendarCache.Update())?.First(c => c.Weekday.Id == Weekday.ToString());
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

    private async void ImageCard_Click(object sender, RoutedEventArgs e) {
        ImageCard  card = (ImageCard)sender;
        AirSubject tag  = (AirSubject)card.Tag;
        await Launcher.LaunchUriAsync(tag.Url);
    }
}
