using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class UserCollectionListItem : ContentControl {
    [ObservableProperty]
    private UserCollection? collection;

    public UserCollectionListItem() {
        InitializeComponent();
    }
}