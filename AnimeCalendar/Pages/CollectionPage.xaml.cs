using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Data;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using System.Collections.Generic;
using System.Linq;

namespace AnimeCalendar.Pages;

[ObservableObject]
public sealed partial class CollectionPage : Page {
    private static Cache<User> bgmUserCache = new(() => BgmApiServices.UserApi.GetMe());

    [ObservableProperty]
    private IEnumerable<UserCollection> collections = [];

    private bool isFetchingData;
    public bool IsFetchingData {
        get => isFetchingData;
        private set => SetProperty(ref isFetchingData, value);
    }

    public CollectionPage() {
        InitializeComponent();
    }

    private async void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args) {
        var  type = (CollectionType)int.Parse((string)sender.SelectedItem.Tag);

        IsFetchingData = true;
        Collections = [];

        User user = await bgmUserCache.GetValueAsync();
        var pagedCollections = await BgmApiServices.CollectionApi.GetCollections(user.Username, type: type);

        IsFetchingData = false;
        Collections = pagedCollections.Data;
    }
}
