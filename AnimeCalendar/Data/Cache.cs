using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Threading.Tasks;

namespace AnimeCalendar.Data;

internal partial class Cache<T> : ObservableObject where T : notnull {
    [ObservableProperty]
    private T? value;

    private Task<T?>? updateTask;

    public bool IsUpdating => updateTask?.Status == TaskStatus.Running;

    private Func<Task<T?>> Updater { get; init; }

    public DateTime LastUpdated { get; private set; }

    public TimeSpan Interval { get; init; }

    public Cache(T? value, Func<Task<T?>> updater, TimeSpan interval) {
        Value    = value;
        Updater  = updater;
        Interval = interval;
    }

    public static async Task<Cache<T>> Create(Func<Task<T?>> updater, TimeSpan interval) {
        Cache<T> cache = new(default(T?), updater, interval);
        await cache.ForceUpdate();
        return cache;
    }

    partial void OnValueChanged(T? value) {
        LastUpdated = DateTime.Now;
    }

    public async Task<T?> ForceUpdate()
        => Value = await (updateTask = Updater());
    
    public async Task<T?> Update() {
        if (IsUpdating) return await updateTask!;

        if (Value == null || DateTime.Now >= LastUpdated + Interval)
            return await ForceUpdate();
        return Value;
    }

    public async Task<T?> Get() {
        if (IsUpdating) return await updateTask!;
        return Value;
    }
}
