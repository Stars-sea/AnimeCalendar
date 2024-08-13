using System.Xml;
using System.Xml.Serialization;

namespace AnimeCalendar.Api.Mikanime.Schemas;

[XmlRoot("rss")]
public class RssRoot {
    [XmlElement("channel")]
    public Channel? Channel { get; set; }

    [XmlAttribute("version")]
    public string? VersionString { get; set; }

    [XmlIgnore]
    public Version? Version => VersionString == null ? null : Version.Parse(VersionString);
}