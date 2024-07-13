using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class BgmUserInfo : UserControl {
    [ObservableProperty]
    private User? user;

    public BgmUserInfo() {
        InitializeComponent();
    }
}
