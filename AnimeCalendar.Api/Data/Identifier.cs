namespace AnimeCalendar.Api.Data;

public record Identifier(string Name, int Id) {
    public static implicit operator Identifier(KeyValuePair<string, int> pair)
        => new Identifier(pair.Key, pair.Value);

    public static explicit operator KeyValuePair<string, int>(Identifier identifier)
        => new KeyValuePair<string, int>(identifier.Name, identifier.Id);
}

public record AnimeIdentifier(string Name, int? Id, Website Website) : IAnime, IEquatable<IAnime> {
    public AnimeIdentifier(IAnime anime) : this(anime.Name, anime.Id, anime.Website) { } 

    public bool Equals(IAnime? other) {
        if (other == null) return false;
        if (Equals(this, other)) return true;

        if (Website == other.Website && Id != null && Id == other.Id)
            return true;

        return Website != other.Website &&
            !string.IsNullOrEmpty(Name) &&
            Name.Trim().Equals(other.Name.Trim());
    }
}
