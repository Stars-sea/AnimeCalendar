using System.Xml.Serialization;

namespace AnimeCalendar.Api.Mikanime.Rss;

public class Channel {
    [XmlElement("title")]
    public string? Title { get; set; }

    [XmlElement("link")]
    public string? Link { get; set; }

    [XmlElement("description")] 
    public string? Description { get; set; }

    [XmlElement("item", Type = typeof(AnimeItem))]
    public AnimeItem[]? Items { get; set; }
}
