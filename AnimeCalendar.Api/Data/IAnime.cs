namespace AnimeCalendar.Api.Data;

public enum Website { Bangumi, Mikanime }

public interface IAnime {
    public string   Name        { get; }
    public string   AutoName    { get; }
    public int?     Id          { get; }
    public Website  Website     { get; }
}

public interface IRatedAnime : IAnime {
    public float Score { get; }
    public int   Rank  { get; }
}
