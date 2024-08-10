using HtmlAgilityPack;

using System.Collections.Frozen;
using System.Text.RegularExpressions;

using static AnimeCalendar.Api.Converter.HtmlDecoder;

namespace AnimeCalendar.Api.Mikanime;

public partial class BangumiPage {
    public HtmlDocument Document { get; init; }

    internal BangumiPage(HtmlDocument document) { 
        Document = document;
    }

    public string GetBangumiName()
        => HtmlDecode(Document.DocumentNode.SelectSingleNode("//p[@class=\"bangumi-title\"]").InnerText).Trim();

    public int? GetBgmSubjectId() {
        Match match = BgmSubjectUrl().Match(Document.Text);
        if (match.Groups.TryGetValue("subjectId", out Group? group))
            return int.Parse(group.Value);
        return null;
    }

    public FrozenDictionary<string, int> GetSubgroups()
        => Document.DocumentNode.SelectNodes("//a[contains(@class, \"subgroup-name\")]").ToFrozenDictionary(
            node => HtmlDecode(node.InnerText).Trim(),
            node => int.Parse(node.Attributes["data-anchor"].Value[1..])
        );

    [GeneratedRegex(@"https://bgm.tv/subject/(?<subjectId>\d+)")]
    private static partial Regex BgmSubjectUrl();
}
