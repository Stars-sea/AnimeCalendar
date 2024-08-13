using AnimeCalendar.Api.Data;

using System.Xml.Serialization;

namespace AnimeCalendar.Api.Mikanime.Rss;

public class Enclosure {
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlAttribute("length", DataType = "long")]
    public long Length { get; set; }

    [XmlAttribute("url")]
    public string? Url { get; set; }
}

public class Torrent {
    [XmlElement("link")]
    public string? Link { get; set; }

    [XmlElement("contentLength")]
    public long? ContentLength { get; set; }

    [XmlElement("pubDate")]
    public DateTime? PubDate { get; set; }
}

public class Episode {
    [XmlElement("guid")]
    public string? Guid { get; set; }

    [XmlElement("link")]
    public string? Link { get; set; }

    [XmlElement("title")]
    public string? Title { get; set; }

    [XmlElement("description")]
    public string? Description { get; set; }

    [XmlElement("enclosure")]
    public Enclosure? Enclosure { get; set; }

    [XmlElement("torrent", Namespace = "https://mikanime.tv/0.1/")]
    public Torrent? Torrent { get; set; }

    [XmlIgnore]
    public SimpleEpisode Simple => new() {
        Name    = Guid ?? throw new NullReferenceException(nameof(Guid)),
        Link    = Link ?? throw new NullReferenceException(nameof(Link)),
        Size    = Description != null ? Description![Description.LastIndexOf('[')..] : throw new NullReferenceException(nameof(Description)),
        Time    = Torrent?.PubDate?.ToString("yyyy/MM/dd HH:mm") ?? throw new NullReferenceException(nameof(Torrent)),
        Magnet  = Enclosure?.Url ?? throw new NullReferenceException(nameof(Enclosure)),
    };
}