using AnimeCalendar.Data;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using System;
using System.Linq;

namespace AnimeCalendar.Pages;

public sealed partial class IndexPage : Page {
    public static IndexPage? Current { get; private set; }

    private NavigationInfoCollection navigations = [];

    public IndexPage() {
        InitializeComponent();

        Current = this;

        Loaded += (_, _) => SelectAnimeList(DateTime.Today.DayOfWeek);
    }

    public void SelectAnimeList(DayOfWeek dayOfWeek) {
        int weekday = (int)dayOfWeek;
        if (weekday == 0) weekday = 7;

        NavView.SelectedItem = Calendar.MenuItems.Cast<NavigationViewItem>()
            .First(i => $"AnimeList#{weekday}".Equals(i.Tag));
    }

    internal void Navigate(NavigationInfo navigation, bool newPage = true) {
        var (page, title, param, transitionInfo) = navigation;
        
        NavView.Header = title;
        ContentFrame.Navigate(page, param, transitionInfo);
        if (newPage) navigations.Navigate(navigation);
    }

    internal bool GoForward() {
        if (navigations.GoForward() is NavigationInfo navigation) {
            Navigate(navigation, false);
            return true;
        }
        return false;
    }

    internal bool GoBack() {
        if (navigations.GoBack() is NavigationInfo navigation) {
            Navigate(navigation, false);
            return true;
        }
        return false;
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        NavigationViewItemBase container = args.SelectedItemContainer;

        if (container == Calendar) return;

        string[] tags = ((string)container.Tag).Split('#');

        Type    page  = Type.GetType($"AnimeCalendar.Pages.{tags[0]}Page", true)!;
        string? param = tags.Length > 1 ? tags[1] : null;

        Navigate(new NavigationInfo(page, (string)container.Content, param));
    }

    private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) {
        GoBack();
    }

    private void NavView_KeyUp(object sender, KeyRoutedEventArgs e) {
        // TODO: ÐÞ¸´²»¼ì²â°´¼ü
        if (e.Key == Windows.System.VirtualKey.GoForward && ContentFrame.CanGoForward)
            GoForward();
        else if (e.Key == Windows.System.VirtualKey.GoBack && ContentFrame.CanGoBack)
            GoBack();
        else if (e.Key == Windows.System.VirtualKey.GoHome)
            SelectAnimeList(DateTime.Today.DayOfWeek);
    }
}
