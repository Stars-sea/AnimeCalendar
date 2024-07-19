using AnimeCalendar.Api.Bangumi.Schemas;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls;

[ObservableObject]
public sealed partial class SubjectListItem : ContentControl {
    [ObservableProperty]
    private AbstractSubject? subject;

    public SubjectListItem() {
        InitializeComponent();
    }
}
