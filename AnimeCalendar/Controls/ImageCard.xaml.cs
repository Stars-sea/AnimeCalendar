using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace AnimeCalendar.Controls;

public sealed partial class ImageCard : ContentControl {
    public ImageSource? IconSource {
        get { return (ImageSource?)GetValue(IconSourceProperty); }
        set { SetValue(IconSourceProperty, value); }
    }

    public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.Register("IconSource", typeof(ImageSource),
            typeof(ImageCard), new PropertyMetadata(null));

    public event RoutedEventHandler? Click;

    public ImageCard() {
        InitializeComponent();
    }

    public void OnClicked(object sender, RoutedEventArgs e) {
        Click?.Invoke(this, e);
    }
}
