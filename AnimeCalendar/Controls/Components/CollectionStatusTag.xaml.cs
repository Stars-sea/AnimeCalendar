using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Data;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls.Components;

[ObservableObject]
public sealed partial class CollectionStatusTag : UserControl {
    [ObservableProperty]
    private int subjectId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CollectionStatus))]
    [NotifyPropertyChangedFor(nameof(WatchProgressString))]
    private UserCollection? collection;

    private CollectionStatus? CollectionStatus => Collection != null 
        ? CollectionStatus.Wrap(Collection.Type)
        : null;

    private string? WatchProgressString => Collection != null
        ? $"观看了 {Collection.EpStatus} 话, 共 {Collection.Subject.Eps} 话"
        : null;

    public CollectionStatusTag() {
        InitializeComponent();
        Loaded                   += CollectionStatusTag_Loaded;
        BgmUserCache.UserChanged += BgmUserCache_UserChanged;
    }

    private async void UpdateStatus() {
        if (BgmUserCache.Instance.User == null)
            return;
        try {
            Collection = await BgmApiServices.CollectionApi.GetCollection(
                BgmUserCache.Instance.User.Username,
                SubjectId
            );
        } catch { }
    }

    private void CollectionStatusTag_Loaded(object sender, RoutedEventArgs e) {
        BgmUserCache_UserChanged(BgmUserCache.Instance.User);
    }

    private void BgmUserCache_UserChanged(User? obj) {
        Visibility = obj == null
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    partial void OnSubjectIdChanged(int value) => UpdateStatus();
}