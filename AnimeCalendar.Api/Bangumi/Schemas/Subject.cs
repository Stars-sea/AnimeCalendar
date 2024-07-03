using System;

namespace AnimeCalendar.Api.Bangumi.Schemas;

public enum SubjectType: int {
    Book    = 1,
    Anime   = 2,
    Music   = 3,
    Game    = 4,
    Reality = 6
}

public class BaseSubject {
    public int          Id      { get; init; }
    public SubjectType  Type    { get; init; }
    public string       Name    { get; init; }
    public string       NameCn  { get; init; }
    public string       Summary { get; init; }
    public RatingType   Rating  { get; init; }
    public ImageURL     Images  { get; init; }
    public Collection   Collection { get; init; }
}

public sealed class Subject : BaseSubject {
    public DateTime     Date            { get; init; }
    public int          Volumes         { get; init; }
    public int          Eps             { get; init; }
    public Tag[]        Tags            { get; init; }
    public bool         Nsfw            { get; init; }
    public bool         Locked          { get; init; }
    public string       Platform        { get; init; }
    public Infobox[]    Infobox         { get; init; }
    public int          TotleEpisodes   { get; init; }
}
