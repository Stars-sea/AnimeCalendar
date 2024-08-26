using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Data;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System.Collections.Generic;
using System.Linq;

namespace AnimeCalendar.Pages;

[ObservableObject]
public sealed partial class CollectionPage : Page, IRecipient<PropertyChangedMessage<User?>> {
    [ObservableProperty]
    private IEnumerable<UserCollection> collections = [];

    private bool IsLoggedIn => BgmUserCache.Instance.User != null;

    private bool isFetchingData;
    public bool IsFetchingData {
        get => isFetchingData;
        private set => SetProperty(ref isFetchingData, value);
    }

    public CollectionPage() {
        InitializeComponent();
    }

    public void Receive(PropertyChangedMessage<User?> message)
        => OnPropertyChanged(nameof(IsLoggedIn));

    private async void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args) {
        var type = (CollectionType)int.Parse((string)sender.SelectedItem.Tag);

        IsFetchingData = true;
        Collections = [];

        User user = BgmUserCache.Instance.User!;
        var collections = await BgmApiServices.CollectionApi.GetCollections(user.Username, type: type);

        IsFetchingData = false;
        Collections = collections.Data;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
        var collection = e.AddedItems.FirstOrDefault() as UserCollection;
        if (collection == null) return;

        NavigationInfo navigation = new(typeof(SubjectDetailPage), collection.SubjectId, null);
        IndexPage.Current!.Navigate(navigation);
    }

    private void OnLoginButtonClick(object sender, RoutedEventArgs e) {
        NavigationInfo navigation = new(typeof(AccountSettingsPage), null, "AccountSettings");
        IndexPage.Current!.Navigate(navigation, false);
    }
}
