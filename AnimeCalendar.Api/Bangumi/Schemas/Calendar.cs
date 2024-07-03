using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeCalendar.Api.Bangumi.Schemas;

public sealed class AirSubject : BaseSubject {
    public string   Url         { get; init; }
    public string   AirDate     { get; init; }
    public int      AirWeekday  { get; init; }
    public int      Rank        { get; init; }
}

public record Weekday(string En, string Cn, string Ja, string Id);

public record Calendar(
    Weekday         Weekday,
    AirSubject[]    Items
);
