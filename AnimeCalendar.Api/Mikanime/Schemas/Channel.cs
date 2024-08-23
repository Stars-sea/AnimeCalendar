using AnimeCalendar.Api.Data;

using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace AnimeCalendar.Api.Mikanime.Schemas;

public partial class Channel : IAnime {
    [XmlElement("title")]
    public string? Title { get; set; }

    [XmlElement("link")]
    public string? Link { get; set; }

    [XmlElement("description")]
    public string? Description { get; set; }

    [XmlElement("item", Type = typeof(Episode))]
    public Episode[]? Items { get; set; }

    [XmlIgnore]
    public string Name => Title != null
        ? Title.Replace("Mikan Project - ", "").Replace("搜索结果:", "").Trim()
        : "";

    [XmlIgnore]
    public string AutoName => Name;

    [XmlIgnore]
    public int? Id {
        get {
            if (Link != null && BangumiId().Match(Link).Groups.TryGetValue("id", out var id))
                return int.Parse(id.Value);
            return null;
        }
    }

    [XmlIgnore] public Website Website => Website.Mikanime;

    public bool Equals(IAnime? other) => new AnimeIdentifier(this).Equals(other);

    [GeneratedRegex(@"bangumiId=(?<id>\d+)", RegexOptions.IgnoreCase)]
    private partial Regex BangumiId();
}
