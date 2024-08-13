namespace AnimeCalendar.Api.Data;

public record struct Identifier(string Name, int Id) {
    public static implicit operator Identifier(KeyValuePair<string, int> pair)
        => new Identifier(pair.Key, pair.Value);

    public static explicit operator KeyValuePair<string, int>(Identifier identifier)
        => new KeyValuePair<string, int>(identifier.Name, identifier.Id);
}
