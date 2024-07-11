using Microsoft.UI.Xaml.Controls;

using System;

namespace AnimeCalendar.Data;

public record GlobalInfo(
    string Title,
    string Message,
    InfoBarSeverity Severity,
    TimeSpan Duration
) {
    public static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(2.5);

    public static GlobalInfo Information(string title, string message, TimeSpan duration)
        => new(title, message, InfoBarSeverity.Informational, duration);

    public static GlobalInfo Information(string title, string message)
        => new(title, message, InfoBarSeverity.Informational, DefaultDuration);

    public static GlobalInfo Success(string title, string message, TimeSpan duration)
        => new(title, message, InfoBarSeverity.Success, duration);

    public static GlobalInfo Success(string title, string message)
        => new(title, message, InfoBarSeverity.Success, DefaultDuration);

    public static GlobalInfo Warning(string title, string message, TimeSpan duration)
        => new(title, message, InfoBarSeverity.Warning, duration);

    public static GlobalInfo Warning(string title, string message)
        => new(title, message, InfoBarSeverity.Warning, DefaultDuration);

    public static GlobalInfo Error(string title, string message, TimeSpan duration)
        => new(title, message, InfoBarSeverity.Error, duration);

    public static GlobalInfo Error(string title, string message)
        => new(title, message, InfoBarSeverity.Error, DefaultDuration);

    public static GlobalInfo Error(string title, string message, Exception exception)
        => Error(title, $"{message}\n{exception.GetType().Name}: {exception.Message}");

    public static GlobalInfo Error(string title, Exception exception)
        => Error(title, $"{exception.GetType().Name}: {exception.Message}");

    public static GlobalInfo Error(Exception exception)
        => Error(exception.GetType().Name, exception.Message);

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