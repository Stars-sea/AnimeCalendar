using AnimeCalendar.Api.Bangumi.Schemas;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

using System;

namespace AnimeCalendar.Data;

internal partial class CollectionStatus2ColorCvrt : DependencyObject, IValueConverter {
    #region Do
    public Brush BgDo {
        get { return (Brush)GetValue(BgDoProperty); }
        set { SetValue(BgDoProperty, value); }
    }

    public static readonly DependencyProperty BgDoProperty =
        DependencyProperty.Register("BgDo", typeof(Brush),
            typeof(CollectionStatus2ColorCvrt), new PropertyMetadata(new SolidColorBrush()));

    public Brush FgDo {
        get { return (Brush)GetValue(FgDoProperty); }
        set { SetValue(FgDoProperty, value); }
    }

    public static readonly DependencyProperty FgDoProperty =
        DependencyProperty.Register("FgDo", typeof(Brush),
            typeof(CollectionStatus2ColorCvrt), new PropertyMetadata(new SolidColorBrush()));
    #endregion

    #region Wish
    public Brush BgWish {
        get { return (Brush)GetValue(BgWishProperty); }
        set { SetValue(BgWishProperty, value); }
    }

    public static readonly DependencyProperty BgWishProperty =
        DependencyProperty.Register("BgWish", typeof(Brush),
            typeof(CollectionStatus2ColorCvrt), new PropertyMetadata(new SolidColorBrush()));

    public Brush FgWish {
        get { return (Brush)GetValue(FgWishProperty); }
        set { SetValue(FgWishProperty, value); }
    }

    public static readonly DependencyProperty FgWishProperty =
        DependencyProperty.Register("FgWish", typeof(Brush),
            typeof(CollectionStatus2ColorCvrt), new PropertyMetadata(new SolidColorBrush()));
    #endregion

    #region Collect
    public Brush BgCollect {
        get { return (Brush)GetValue(BgCollectProperty); }
        set { SetValue(BgCollectProperty, value); }
    }

    public static readonly DependencyProperty BgCollectProperty =
        DependencyProperty.Register("BgCollect", typeof(Brush),
            typeof(CollectionStatus2ColorCvrt), new PropertyMetadata(new SolidColorBrush()));

    public Brush FgCollect {
        get { return (Brush)GetValue(FgCollectProperty); }
        set { SetValue(FgCollectProperty, value); }
    }

    public static readonly DependencyProperty FgCollectProperty =
        DependencyProperty.Register("FgCollect", typeof(Brush),
            typeof(CollectionStatus2ColorCvrt), new PropertyMetadata(new SolidColorBrush()));
    #endregion

    #region OnHold
    public Brush BgOnHold {
        get { return (Brush)GetValue(BgOnHoldProperty); }
        set { SetValue(BgOnHoldProperty, value); }
    }

    public static readonly DependencyProperty BgOnHoldProperty =
        DependencyProperty.Register("BgOnHold", typeof(Brush),
            typeof(CollectionStatus2ColorCvrt), new PropertyMetadata(new SolidColorBrush()));

    public Brush FgOnHold {
        get { return (Brush)GetValue(FgOnHoldProperty); }
        set { SetValue(FgOnHoldProperty, value); }
    }

    public static readonly DependencyProperty FgOnHoldProperty =
        DependencyProperty.Register("FgOnHold", typeof(Brush),
            typeof(CollectionStatus2ColorCvrt), new PropertyMetadata(new SolidColorBrush()));
    #endregion

    #region Dropped
    public Brush BgDropped {
        get { return (Brush)GetValue(BgDroppedProperty); }
        set { SetValue(BgDroppedProperty, value); }
    }

    public static readonly DependencyProperty BgDroppedProperty =
        DependencyProperty.Register("BgDropped", typeof(Brush),
            typeof(CollectionStatus2ColorCvrt), new PropertyMetadata(new SolidColorBrush()));

    public Brush FgDropped {
        get { return (Brush)GetValue(FgDroppedProperty); }
        set { SetValue(FgDroppedProperty, value); }
    }

    public static readonly DependencyProperty FgDroppedProperty =
        DependencyProperty.Register("FgDropped", typeof(Brush),
            typeof(CollectionStatus2ColorCvrt), new PropertyMetadata(new SolidColorBrush()));
    #endregion

    public object Convert(object value, Type targetType, object parameter, string language) {
        var status = value as CollectionStatus;
        if (status == null) return new SolidColorBrush();

        if ((object?)parameter != null) {
            return status.Type switch {
                CollectionType.Do      => FgDo,
                CollectionType.Wish    => FgWish,
                CollectionType.Collect => FgCollect,
                CollectionType.OnHold  => FgOnHold,
                CollectionType.Dropped => FgDropped,
                _                      => null
            } ?? new SolidColorBrush();
        }

        return status.Type switch {
            CollectionType.Do      => BgDo,
            CollectionType.Wish    => BgWish,
            CollectionType.Collect => BgCollect,
            CollectionType.OnHold  => BgOnHold,
            CollectionType.Dropped => BgDropped,
            _                      => null
        } ?? new SolidColorBrush();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        throw new NotImplementedException();
    }
}
