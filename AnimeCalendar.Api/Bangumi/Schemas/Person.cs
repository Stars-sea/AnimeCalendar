namespace AnimeCalendar.Api.Bangumi.Schemas;

public enum PersonType : int {
    Person  = 1,
    Company = 2,
    Group   = 3
}

public record Person(
    int         Id,
    PersonType  Type,
    string      Name,
    string      Relation,
    ImageURL    Images,
    string[]    Career
);

public record Actor(
    int         Id,
    PersonType  Type,
    string      Name,
    string      Relation,
    ImageURL    Images,
    string[]    Career,
    string      ShortSummary,
    bool        Locked
) : Person(Id, Type, Name, Relation, Images, Career);
