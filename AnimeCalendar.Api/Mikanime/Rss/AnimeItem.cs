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

public class AnimeItem {
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
}