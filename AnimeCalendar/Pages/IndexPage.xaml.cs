using Microsoft.UI.Xaml.Controls;

using System;

namespace AnimeCalendar.Pages;

public sealed partial class IndexPage : Page {
    public IndexPage() {
        InitializeComponent();
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
        NavigationViewItemBase container = args.InvokedItemContainer;

        sender.Header = container.Content as string;
        string[] tags = ((string)container.Tag).Split('#');
        
        Type    page  = Type.GetType($"AnimeCalendar.Pages.{tags[0]}Page", true)!;
        string? param = tags.Length > 1 ? tags[1] : null;
        ContentFrame.Navigate(page, param);
    }
}
