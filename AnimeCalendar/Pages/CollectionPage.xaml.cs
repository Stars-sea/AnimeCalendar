using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Data;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System.Collections.Generic;

namespace AnimeCalendar.Pages;

[ObservableObject]
public sealed partial class CollectionPage : Page {
    [ObservableProperty]
    private IEnumerable<UserCollection> collections = [];

    private bool LoggedIn => BgmUserCache.Instance.User != null;

    private bool isFetchingData;
    public bool IsFetchingData {
        get => isFetchingData;
        private set => SetProperty(ref isFetchingData, value);
    }

    public CollectionPage() {
        InitializeComponent();
    }

    private async void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args) {
        var type = (CollectionType)int.Parse((string)sender.SelectedItem.Tag);

        IsFetchingData = true;

        Collections = [];
        User user = BgmUserCache.Instance.User!;
        var pagedCollections = await BgmApiServices.CollectionApi.GetCollections(user.Username, type: type);

        IsFetchingData = false;
        Collections = pagedCollections.Data;
    }

    private void LoginButtonClick(object sender, RoutedEventArgs e) {
        NavigationInfo navigation = new(typeof(AccountSettingsPage), null, null, "AccountSettings");
        IndexPage.Current!.Navigate(navigation, false);
    }
}
