using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Controls.Base;
using AnimeCalendar.Data;
using AnimeCalendar.UI;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;

using System.Linq;

namespace AnimeCalendar.Controls.Components;

[ObservableObject]
public sealed partial class CollectionStatusSelector : TasksCountableControl {
    private readonly CollectionStatus[] Statuses = [
        new("在看", CollectionType.Do),
        new("想看", CollectionType.Wish),
        new("看过", CollectionType.Collect),
        new("搁置", CollectionType.OnHold),
        new("抛弃", CollectionType.Dropped)
    ];

    [ObservableProperty]
    private int subjectId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SubControlVisibility))]
    private CollectionStatus? collectionStatus;

    public Visibility SubControlVisibility 
        => CollectionStatus == null
            ? Visibility.Collapsed
            : Visibility.Visible;

    public CollectionStatusSelector() {
        InitializeComponent();
        Loaded                   += OnLoaded;
        BgmUserCache.UserChanged += BgmUserCache_UserChanged;
    }

    private async void DownloadCollectionType() {
        if (BgmUserCache.Instance.User == null)
            return;
        string username = BgmUserCache.Instance.User.Username;

        RunningTasksCount++;
        try {
            var collection = await BgmApiServices.CollectionApi.GetCollection(username, SubjectId);
            CollectionStatus = Statuses.FirstOrDefault(t => t.Type == collection.Type);
        }
        catch {
            CollectionStatus = null;
        }
        RunningTasksCount--;
    }

    private async void UploadCollectionType(CollectionType? type) {
        RunningTasksCount++;
        await BgmApiServices.CollectionApi.PostCollection(SubjectId, new() {
            Type = type
        });
        RunningTasksCount--;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
        => BgmUserCache_UserChanged(BgmUserCache.Instance.User);

    private void BgmUserCache_UserChanged(User? obj)
        => Visibility = obj == null ? Visibility.Collapsed : Visibility.Visible;

    private void CancelCollectionClick(object sender, RoutedEventArgs e) {
        // TODO: see https://github.com/bangumi/server/pull/556
        App.MainWindow.Pop(PopInfo.Info("无法操作", "Bangumi API 尚不支持此操作, 请转到网页操作"));
    }

    partial void OnSubjectIdChanged(int value) => DownloadCollectionType();

    partial void OnCollectionStatusChanged(CollectionStatus? value) {
        UploadCollectionType(value?.Type);
    }
}
