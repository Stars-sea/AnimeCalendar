namespace AnimeCalendar.Api.Bangumi.Schemas;

public record Images(
    string Large,
    string Common,
    string Medium,
    string Small,
    string Grid
);

public record Infobox(string Key, object Value);

public record RatingType(
    int     Rank,
    int     Total,
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

public record PagedObject<T>(
    int Total,
    int Limit,
    int Offset,
    T[] Data
);
