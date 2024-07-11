using System;
using System.Threading.Tasks;

using Windows.Foundation;

namespace AnimeCalendar.Utils;

internal static class TaskHelper {
    public static async Task<object?> WrapTask<T>(this Task<T> task) => await task;
    public static async Task<object?> WrapTask<T>(this ValueTask<T> task) => await task;
    public static async Task<object?> WrapTask<T>(this IAsyncOperation<T> task) => await task;
}
