using Microsoft.UI.Xaml.Controls;

using System;

namespace AnimeCalendar.Data;

public record GlobalInfo(
    string Title,
    string Message,
    InfoBarSeverity Severity,
    TimeSpan Duration
) {
    public static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(1.5);

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
}