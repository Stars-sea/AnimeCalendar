using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Threading.Tasks;

namespace AnimeCalendar.Data;

internal sealed class Cache<T> : ObservableObject {
    public Func<Task<T>> TaskGetter { get; init; }

    private Task<T> currentTask;
    public Task<T> CurrentTask {
        get => currentTask;
        private set {
            currentTask = value;
            value.ContinueWith(t => {
                Value = t.Result;
                OnPropertyChanged(nameof(Value));
            });
        }
    }

    public T? Value { get; private set; }

#pragma warning disable CS8618
    public Cache(Func<Task<T>> taskGetter) {
        TaskGetter  = taskGetter;
        CurrentTask = TaskGetter();
    }
#pragma warning restore CS8618

    public Task<T> GetValueAsync()
        => Value != null ? Task.FromResult(Value) : currentTask;

    public static Cache<T> Create(Task<T> task) => new(() => task);
}
