using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Bangumi.Schemas;
using AnimeCalendar.Data;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Threading.Tasks;

namespace AnimeCalendar.Controls.Components;

[ObservableObject]
public sealed partial class EpCollectionButton : UserControl {
    [ObservableProperty]
    private EpCollection? epCollection;

    [ObservableProperty]
    private bool isChecked;

    private static ICollectionApi Api => BgmApiServices.CollectionApi;

    public event RoutedEventHandler? Syncing;
    public event RoutedEventHandler? Synced;

    public EpCollectionButton() {
        InitializeComponent();
    }

    private async Task SyncCollectionStatus(EpCollectionType type) {
        if (EpCollection == null)
            throw new NullReferenceException(nameof(EpCollection));

        int epId = EpCollection.Episode.Id;

        Syncing?.Invoke(this, new());
        try {
            await Api.PutEpCollectionType(epId, new(null, type));
            EpCollection = await Api.GetEpisode(epId);
        }
        catch (Exception ex) {
            App.MainWindow.Pop(PopInfo.Fail("²Ù×÷Ê§°Ü", ex));
            OnPropertyChanged(nameof(EpCollection));
        }
        Synced?.Invoke(this, new());
    }

    partial void OnEpCollectionChanged(EpCollection? value) {
        IsChecked = EpCollection != null
            ? EpCollection.Type != EpCollectionType.NoCollect
            : false;
    }

    private async void ToggleButton_Click(object sender, RoutedEventArgs e)
        => await SyncCollectionStatus(IsChecked
            ? EpCollectionType.NoCollect
            : EpCollectionType.Collect
        );
}
