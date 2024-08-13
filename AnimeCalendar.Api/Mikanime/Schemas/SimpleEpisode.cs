using System.Text.RegularExpressions;

namespace AnimeCalendar.Api.Mikanime.Schemas;

public partial struct SimpleEpisode {
    public string Name      { get; init; }
    public string Link      { get; init; }
    public string Size      { get; init; }
    public string Time      { get; init; }
    public string Magnet    { get; init; }

    public string PureName => TagRegex().Replace(Name, "").Trim();

    public string[] Tags => TagRegex().Matches(Name).Select(x => x.Value).ToArray();

    [GeneratedRegex(@"[\[\(【].+?[】\)\]]")]
    private partial Regex TagRegex();
}
