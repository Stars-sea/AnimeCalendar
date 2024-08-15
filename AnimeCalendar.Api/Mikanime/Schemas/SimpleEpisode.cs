using System.Text.RegularExpressions;

namespace AnimeCalendar.Api.Mikanime.Schemas;

public partial record struct SimpleEpisode(
    string Name,
    string Link,
    string Size,
    string Time,
    string Magnet
) {
    public string PureName {
        get {
            string[] names = AttributeRegex()
                .Split(SuffixRegex().Replace(Name, ""))
                .Select(s => s.Trim()).ToArray();

            string? firstAttr = Attributes.FirstOrDefault();

            return names
                .Where(n => !string.Equals(firstAttr, n))
                .Where(n => !ResolutionRegex().IsMatch(n))
                .MaxBy(n => n.Length) ?? string.Join(' ', names);
        }
    }

    public string[] Attributes => AttributeRegex().Matches(Name).Select(x => x.Value).ToArray();

    [GeneratedRegex(@"(\[[^\n/]+?\])|(\([^\n/]+?\))|(【[^\n/]+?】)|(（[^\n/]+?）)")]
    private static partial Regex AttributeRegex();

    [GeneratedRegex(@"\.\w+$")]
    private static partial Regex SuffixRegex();

    [GeneratedRegex(@"(\d{3,4}p)|(\d{3,4}[x×]\d{3,4})", RegexOptions.IgnoreCase)]
    private static partial Regex ResolutionRegex();
}
