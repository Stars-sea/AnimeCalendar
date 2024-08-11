using AnimeCalendar.Api.Converter;

using HtmlAgilityPack;

using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace AnimeCalendar.Api.Mikanime;

public partial class BangumiPage {
    public HtmlDocument Document { get; init; }

    public HtmlNode RootNode => Document.DocumentNode;

    internal BangumiPage(HtmlDocument document) { 
        Document = document;
    }

    public string BangumiName
        => RootNode.SelectSingleNode("//p[@class=\"bangumi-title\"]").InnerText.Trim().UnicodeUnescape();

    public int? BgmSubjectId {
        get {
            Match match = BgmSubjectUrl().Match(Document.Text);
            if (match.Groups.TryGetValue("subjectId", out Group? group))
                return int.Parse(group.Value);
            return null;
        }
    }

    public FrozenDictionary<string, int> Subgroups
        => RootNode.SelectNodes("//a[contains(@class, \"subgroup-name\")]").ToFrozenDictionary(
            node => node.InnerText.Trim().UnicodeUnescape(),
            node => int.Parse(node.Attributes["data-anchor"].Value[1..])
        );

    [GeneratedRegex(@"https://bgm.tv/subject/(?<subjectId>\d+)")]
    private static partial Regex BgmSubjectUrl();
}
