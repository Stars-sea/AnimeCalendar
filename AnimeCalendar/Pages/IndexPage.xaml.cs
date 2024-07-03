using Microsoft.UI.Xaml.Controls;

using System;
using System.Diagnostics;

namespace AnimeCalendar.Pages;

/// <summary>
/// Index Page
/// </summary>
public sealed partial class IndexPage : Page {
    public IndexPage() {
        InitializeComponent();
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
        string[] tags = ((string)args.InvokedItemContainer.Tag).Split('#');

        Type page = tags[0] switch {
            "CollectionPage"        => typeof(CollectionPage),
            "CalendarRootPage"      => typeof(CalendarRootPage),
            "AnimeListPage"         => typeof(AnimeListPage),
            "AccountSettingsPage"   => typeof(AccountSettingsPage),
            "Settings"              => typeof(SettingsPage),
            _ => null
        };

        ContentFrame.Navigate(page, tags.Length > 1 ? tags[1] : null);
    }
}
