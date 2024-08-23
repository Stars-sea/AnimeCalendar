using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Controls.Base;
using AnimeCalendar.Data;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

using Microsoft.UI.Xaml;

namespace AnimeCalendar.Controls.Components;

[ObservableObject]
public sealed partial class CollectionStatusSelector : TasksCountableControl, IRecipient<PropertyChangedMessage<User?>> {
    [ObservableProperty]
    private int subjectId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SubControlVisibility))]
    private CollectionStatus? collectionStatus;

    private Visibility SubControlVisibility 
        => CollectionStatus == null
            ? Visibility.Collapsed
            : Visibility.Visible;

    public CollectionStatusSelector() {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void DownloadCollectionType() {
        if (BgmUserCache.Instance.User == null)
            return;
        string username = BgmUserCache.Instance.User.Username;

        RunningTasksCount++;
        try {
            var collection = await BgmApiServices.CollectionApi.GetCollection(username, SubjectId);
            CollectionStatus = CollectionStatus.Wrap(collection.Type);
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
        => Visibility = BgmUserCache.Instance.User == null
            ? Visibility.Collapsed 
            : Visibility.Visible;

    public void Receive(PropertyChangedMessage<User?> message)
        => Visibility = message.NewValue == null
            ? Visibility.Collapsed
            : Visibility.Visible;

    private void CancelCollectionClick(object sender, RoutedEventArgs e) {
        // TODO: see https://github.com/bangumi/server/pull/556
        App.MainWindow.Pop(PopInfo.Info("无法操作", "Bangumi API 尚不支持此操作, 请转到网页操作"));
    }

    partial void OnSubjectIdChanged(int value) => DownloadCollectionType();

    partial void OnCollectionStatusChanged(CollectionStatus? value) {
        UploadCollectionType(value?.Type);
    }
}
