using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class LoginCard : UserControl {
    [ObservableProperty]
    private ImageSource iconSource;

    [ObservableProperty]
    private string displayText;

    public event RoutedEventHandler Click;

    public LoginCard() {
        InitializeComponent();
    }

    private void OnClicked(object sender, RoutedEventArgs e) {
        Click?.Invoke(this, e);
    }
}