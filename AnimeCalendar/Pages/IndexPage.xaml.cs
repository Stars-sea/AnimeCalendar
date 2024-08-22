using AnimeCalendar.Data;

using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using System;
using System.Linq;

namespace AnimeCalendar.Pages;

public sealed partial class IndexPage : Page {
    public static IndexPage? Current { get; private set; }

    private NavigationInfoCollection navigations = [];

    private bool ignoreSelectionChanged = false;

    public IndexPage() {
        InitializeComponent();

        Current = this;

        Loaded += (_, _) => SelectTimeline(DateTime.Today.DayOfWeek);
    }

    public void SelectTimeline(DayOfWeek dayOfWeek) {
        int weekday = (int)dayOfWeek;
        if (weekday == 0) weekday = 7;

        NavView.SelectedItem = Calendar.MenuItems.Cast<NavigationViewItem>()
            .First(i => $"{nameof(TimelinePage)}#{weekday}".Equals(i.Tag));
    }

    internal void Navigate(NavigationInfo navigation, bool newPage = true) {
        var (page, title, param, tag, transitionInfo) = navigation;

        if (tag is string itemTag) {
            ignoreSelectionChanged = true;

            if (itemTag.StartsWith("TimelinePage"))
                NavView.SelectedItem = Calendar.MenuItems
                    .Cast<NavigationViewItem>()
                    .First(i => itemTag.Equals(i.Tag));
            else NavView.SelectedItem = NavView.MenuItems
                    .Concat(NavView.FooterMenuItems)
                    .Cast<NavigationViewItem>()
                    .First(i => itemTag.Equals(i.Tag));

            ignoreSelectionChanged = false; // ±ØÒª²Ù×÷
        }

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
        if (ignoreSelectionChanged) {
            ignoreSelectionChanged = false;
            return;
        }

        NavigationViewItemBase container = args.SelectedItemContainer;

        if (container == Calendar) return;

        string[] tags = ((string)container.Tag).Split('#');

        Type page = Type.GetType(
            tags[0].EndsWith("Page")
                ? $"AnimeCalendar.Pages.{tags[0]}"
                : $"AnimeCalendar.Pages.{tags[0]}Page",
            true
        )!;
        string? param = tags.Length > 1 ? tags[1] : null;

        Navigate(new NavigationInfo(page, (string)container.Content, param, null));
    }

    private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) {
        GoBack();
    }

    private void NavView_PointerPressed(object sender, PointerRoutedEventArgs e) {
        PointerPointProperties properties = e.GetCurrentPoint(NavView).Properties;

        if (properties.IsLeftButtonPressed ||
            properties.IsRightButtonPressed ||
            properties.IsMiddleButtonPressed)
            return;

        bool backPressed = properties.IsXButton1Pressed;
        bool forwardPressed = properties.IsXButton2Pressed;
        if (backPressed ^ forwardPressed) {
            e.Handled = true;
            if (backPressed) GoBack();
            if (forwardPressed) GoForward();
        }
    }
}
