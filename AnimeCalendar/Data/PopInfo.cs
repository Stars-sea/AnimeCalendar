using Microsoft.UI.Xaml.Controls;

using System;

namespace AnimeCalendar.Data;

internal record PopInfo(
    string Title,
    string Message,
    InfoBarSeverity Severity,
    TimeSpan Duration
) {
    public static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(2.5);

    public static PopInfo Info(string title, string message, TimeSpan duration)
        => new(title, message, InfoBarSeverity.Informational, duration);

    public static PopInfo Info(string title, string message)
        => new(title, message, InfoBarSeverity.Informational, DefaultDuration);

    public static PopInfo Succ(string title, string message, TimeSpan duration)
        => new(title, message, InfoBarSeverity.Success, duration);

    public static PopInfo Succ(string title, string message)
        => new(title, message, InfoBarSeverity.Success, DefaultDuration);

    public static PopInfo Warn(string title, string message, TimeSpan duration)
        => new(title, message, InfoBarSeverity.Warning, duration);

    public static PopInfo Warn(string title, string message)
        => new(title, message, InfoBarSeverity.Warning, DefaultDuration);

    public static PopInfo Fail(string title, string message, TimeSpan duration)
        => new(title, message, InfoBarSeverity.Error, duration);

    public static PopInfo Fail(string title, string message)
        => new(title, message, InfoBarSeverity.Error, DefaultDuration);

    public static PopInfo Fail(string title, string message, Exception exception)
        => Fail(title, $"{message}\n{exception.GetType().Name}: {exception.Message}");

    public static PopInfo Fail(string title, Exception exception)
        => Fail(title, $"{exception.GetType().Name}: {exception.Message}");

    public static PopInfo Fail(Exception exception)
        => Fail(exception.GetType().Name, exception.Message);

    public override string ToString() {
        string severityText = Severity switch {
            InfoBarSeverity.Success         => "[SUCC]",
            InfoBarSeverity.Informational   => "[INFO]",
            InfoBarSeverity.Warning         => "[WARN]",
            InfoBarSeverity.Error           => "[FAIL]",
            _                               => "[NONE]"
        };
        return $"{severityText} {Title}: {Message.Replace('\n', ' ')}";
    }
}