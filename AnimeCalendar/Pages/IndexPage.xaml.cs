using Microsoft.UI.Xaml.Controls;

using System;
using System.Linq;

namespace AnimeCalendar.Pages;

public sealed partial class IndexPage : Page {
    public IndexPage() {
        InitializeComponent();

        Loaded += (_, _) => SelectAnimeList(DateTime.Today.DayOfWeek);
    }

    public void SelectAnimeList(DayOfWeek dayOfWeek) {
        int weekday = (int)dayOfWeek;
        if (weekday == 0) weekday = 7;

        NavView.SelectedItem = Calendar.MenuItems.Cast<NavigationViewItem>()
            .First(i => $"AnimeList#{weekday}".Equals(i.Tag));
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        NavigationViewItemBase container = args.SelectedItemContainer;

        if (container == Calendar) return;

        string[] tags = ((string)container.Tag).Split('#');

        Type    page  = Type.GetType($"AnimeCalendar.Pages.{tags[0]}Page", true)!;
        string? param = tags.Length > 1 ? tags[1] : null;

        sender.Header = container.Content;
        ContentFrame.Navigate(page, param);
    }
}
