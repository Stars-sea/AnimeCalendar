using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls.Cards;

[ObservableObject]
public sealed partial class BgmUserCard : UserControl {
    [ObservableProperty]
    private User? user;

    public BgmUserCard() {
        InitializeComponent();
    }
}
