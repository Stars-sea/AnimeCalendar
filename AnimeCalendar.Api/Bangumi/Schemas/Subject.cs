using AnimeCalendar.Api.Converter;

using Newtonsoft.Json;

namespace AnimeCalendar.Api.Bangumi.Schemas;

public enum SubjectType: int {
    Book    = 1,
    Anime   = 2,
    Music   = 3,
    Game    = 4,
    Reality = 6
}

public record BaseSubject(
    int         Id,
    SubjectType Type,
    string      Name,
    string      NameCn,
    Images      Images
);

public record AbstractSubject(
    int         Id,
    SubjectType Type,
    string      Name,
    string      NameCn,
    string      Summary,
    RatingType  Rating,
    Images      Images,
    Collection  Collection
) : BaseSubject(Id, Type, Name, NameCn, Images);

public sealed record AirSubject(
    int         Id,
    SubjectType Type,
    string      Name,
    string      NameCn,
    string      Summary,
    RatingType  Rating,
    Images      Images,
    Collection  Collection,
    Uri         Url,
    string      AirDate,
    int         AirWeekday,
    int         Rank
) : AbstractSubject(Id, Type, Name, NameCn, Summary, Rating, Images, Collection);

public sealed record RelatedSubject(
    int         Id,
    SubjectType Type,
    string      Name,
    string      NameCn,
    Images      Images,
    string      Relation
) : BaseSubject(Id, Type, Name, NameCn, Images);

public record SlimSubject(
    int         Id,
    SubjectType Type,
    string      Name,
    string      NameCn,
    Images      Images,
    string      ShortSummary,
    int         Volumes,
    int         Eps,
    int         CollectionTotal,
    double      Score,
    Tag[]       Tags,

    [JsonConverter(typeof(DateOnlyConverter))]
    DateTime? Date
) : BaseSubject(Id, Type, Name, NameCn, Images);

public sealed record Subject(
    int         Id,
    SubjectType Type,
    string      Name,
    string      NameCn,
    string      Summary,
    RatingType  Rating,
    Images      Images,
    Collection  Collection,
    DateTime    Date,
    int         Volumes,
    int         Eps,
    Tag[]       Tags,
    bool        Nsfw,
    bool        Locked,
    string      Platform,
    Infobox[]   Infobox,
    int         TotleEpisodes
) : AbstractSubject(Id, Type, Name, NameCn, Summary, Rating, Images, Collection);
