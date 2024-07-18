using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls;

public sealed partial class SubjectListItem : ContentControl {
    public AbstractSubject? Subject {
        get { return (AbstractSubject)GetValue(SubjectProperty); }
        set { SetValue(SubjectProperty, value); }
    }

    public static readonly DependencyProperty SubjectProperty =
        DependencyProperty.Register("Subject", typeof(AbstractSubject), 
            typeof(SubjectListItem), new PropertyMetadata(null));


    public string? IconAnimationKey {
        get { return (string?)GetValue(IconAnimationKeyProperty); }
        set { SetValue(IconAnimationKeyProperty, value); }
    }

    public static readonly DependencyProperty IconAnimationKeyProperty =
        DependencyProperty.Register("IconAnimationKey", typeof(string),
            typeof(SubjectListItem), new PropertyMetadata(null));

    public SubjectListItem() {
        InitializeComponent();
    }
}
