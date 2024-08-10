using AnimeCalendar.Api.Mikanime.Rss;

using HtmlAgilityPack;

using Refit;

using System.Collections.Frozen;

using static AnimeCalendar.Api.Converter.HtmlDecoder;

namespace AnimeCalendar.Api.Mikanime;

[Headers(ModuleSettings.USER_AGENT)]
public interface ISearchAnimeApi {
    [Get("/Home/Search")]
    Task<string> SearchRawHtml(string searchStr);

    async Task<HtmlDocument> SearchHtml(string searchStr) {
        string raw = await SearchRawHtml(searchStr);
        HtmlDocument document = new();
        document.LoadHtml(raw);
        return document;
    }

    async Task<FrozenDictionary<string, int>> SearchBangumiIds(string searchStr) {
        HtmlDocument document = await SearchHtml(searchStr);

        /* //*[@id="sk-container"]/div[2]/ul/li/a */
        return document.DocumentNode.SelectNodes("//ul[@class=\"list-inline an-ul\"]/li/a").ToFrozenDictionary(
            node => HtmlDecode(node.SelectSingleNode("//div[@class=\"an-text\"]").InnerText).Trim(),
            node => {
                string path = node.Attributes["href"].Value;
                return int.Parse(path[(path.LastIndexOf('/') + 1)..]);
            }
        );
    }

    [Get("/RSS/Search")]
    Task<RssRoot> SearchRss(string searchStr);
}
