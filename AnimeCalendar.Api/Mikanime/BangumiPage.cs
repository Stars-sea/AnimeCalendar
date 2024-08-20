using AnimeCalendar.Api.Converter;
using AnimeCalendar.Api.Data;
using AnimeCalendar.Api.Mikanime.Schemas;

using HtmlAgilityPack;

using System.Text.RegularExpressions;

namespace AnimeCalendar.Api.Mikanime;

public partial class BangumiPage {
    public HtmlDocument Document { get; init; }

    public HtmlNode RootNode => Document.DocumentNode;

    internal BangumiPage(HtmlDocument document) {
        Document = document;
    }

    public string BangumiName
        => RootNode.SelectSingleNode("//p[@class=\"bangumi-title\"]").InnerText.Decode();

    public int? BgmSubjectId {
        get {
            Match match = BgmSubjectUrl().Match(Document.Text);
            if (match.Groups.TryGetValue("subjectId", out Group? group))
                return int.Parse(group.Value);
            return null;
        }
    }

    public Identifier[] Subgroups
        => RootNode.SelectNodes("//a[contains(@class, \"subgroup-name\")]").Select(node =>
            new Identifier(
                node.InnerText.Decode(),
                int.Parse(node.Attributes["data-anchor"].Value[1..])
            )
        ).ToArray();

    public SimpleEpisode[] GetEpisodes(int subgroupId) {
        HtmlNode table = RootNode.SelectSingleNode($"//div[@id=\"{subgroupId}\"]").NextSibling.NextSibling;
        return table.SelectNodes(".//tr").Skip(1).Select(tr => {
            HtmlNode wrapNode = tr.SelectSingleNode("./td[1]/a[1]");
            return new SimpleEpisode(
                wrapNode.InnerText.Decode(),
                MikanimeServices.BASE_ADDRESS + wrapNode.Attributes["href"].Value,
                tr.SelectSingleNode("./td[2]").InnerText.Decode(),
                tr.SelectSingleNode("./td[3]").InnerText.Decode(),
                tr.SelectSingleNode("./td[1]/a[2]").Attributes["data-clipboard-text"].Value,
                BangumiName
            );
        }).ToArray();
    }

    [GeneratedRegex(@"https://bgm.tv/subject/(?<subjectId>\d+)")]
    private static partial Regex BgmSubjectUrl();
}
