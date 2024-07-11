using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class UserInfo : UserControl {
    [ObservableProperty]
    private User? user;

    private string? username => User != null ? $"@{User.Username}" : null;

    public UserInfo() {
        InitializeComponent();
    }
}
