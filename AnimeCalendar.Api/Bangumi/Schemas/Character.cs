namespace AnimeCalendar.Api.Bangumi.Schemas;

public record Character(
    int     Id,
    string  Name,
    int     Type,   // TODO
    Images  Images,
    string  Relation,
    Actor[] Actors
);
