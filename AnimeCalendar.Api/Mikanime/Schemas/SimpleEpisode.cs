using System.Text.RegularExpressions;

namespace AnimeCalendar.Api.Mikanime.Schemas;

public partial record SimpleEpisode(
    string Name,
    string Link,
    string Size,
    string Time,
    string Magnet,
    string? BangumiName = null
) {
    public string PureName {
        get {
            string[] names = SuffixRegex().Replace(Name, "")
                .Split(Attributes, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return string.Join(' ', names);
        }
    }

    public string[] Attributes {
        get {
            string subgroup = SubgroupRegex().Match(Name).Value;
            var    attrib   = AttributeRegex().Matches(Name.Replace(subgroup, ""))
                .Select(x => x.Value).Distinct().Where(NotContainsBangumiName);
            return new string[] { subgroup }.Concat(attrib).ToArray();
        }
    }

    public static readonly char[] Brackets = ['[', ']', '(', ')', '（', '）', ' '];

    private bool NotContainsBangumiName(string attr) {
        if (BangumiName == null) return true;
        return !BangumiName.Split(' ').Any(s => attr.Contains(s));
    }

    [GeneratedRegex(@"【[^\n/_]+?】|\[[^\n/_]+?\]")]
    private static partial Regex SubgroupRegex();

    [GeneratedRegex(@"\[[^\n/_]+?\]|\([^\n/_]+?\)|（[^\n/_]+?）|★[^\n/_]+?★")]
    private static partial Regex AttributeRegex();

    [GeneratedRegex(@"\.\w+$")]
    private static partial Regex SuffixRegex();
}
