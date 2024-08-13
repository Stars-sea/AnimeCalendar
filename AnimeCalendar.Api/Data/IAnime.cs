using AnimeCalendar.Api.Storage;

namespace AnimeCalendar.Api.Data;

public enum Website { Bangumi, Mikanime }

public interface IAnime {
    public string   Name     { get; }
    public int?     Id       { get; }
    public Website  Website { get; }
}
