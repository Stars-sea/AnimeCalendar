using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeCalendar.Api.Bangumi.Schemas;

public sealed record AirSubject(
    int         Id,
    SubjectType Type,
    string      Name,
    string      NameCn,
    string      Summary,
    RatingType  Rating,
    ImageURL    Images,
    Collection  Collection,
    string      Url,
    string      AirDate,
    int         AirWeekday,
    int         Rank
) : AbstractSubject(Id, Type, Name, NameCn, Summary, Rating, Images, Collection);

public record Weekday(string En, string Cn, string Ja, string Id);

public record Calendar(
    Weekday         Weekday,
    AirSubject[]    Items
);
