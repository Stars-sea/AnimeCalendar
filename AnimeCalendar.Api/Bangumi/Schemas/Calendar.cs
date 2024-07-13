namespace AnimeCalendar.Api.Bangumi.Schemas;

public record Weekday(string En, string Cn, string Ja, string Id);

public record Calendar(
    Weekday         Weekday,
    AirSubject[]    Items
);
