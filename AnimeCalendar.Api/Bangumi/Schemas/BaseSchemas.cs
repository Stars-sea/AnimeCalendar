using System.Collections.Generic;

namespace AnimeCalendar.Api.Bangumi.Schemas;

public record ImageURL(
    string Large,
    string Common,
    string Medium,
    string Small,
    string Grid
);

public record Infobox(string Key, object Value);

public record RatingType(
    int     Rank,
    int     Totle,
    float   Score,
    IReadOnlyDictionary<string, int> Count
);

public record Collection(
    int Wish,
    int Collect,
    int Doing,
    int OnHold,
    int Dropped
);

public record Tag(string Name, int Count);
