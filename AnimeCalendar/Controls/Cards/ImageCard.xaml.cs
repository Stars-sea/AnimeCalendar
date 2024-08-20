using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace AnimeCalendar.Controls.Cards;

[ObservableObject]
public sealed partial class ImageCard : ContentControl {
    [ObservableProperty]
    private ImageSource? iconSource;

    public event RoutedEventHandler? Click;

    public ImageCard() {
        InitializeComponent();
    }

    public void OnClicked(object sender, RoutedEventArgs e) {
        Click?.Invoke(this, e);
    }
}
