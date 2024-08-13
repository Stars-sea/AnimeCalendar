using AnimeCalendar.Api.Converter;
using AnimeCalendar.Api.Data;
using AnimeCalendar.Api.Mikanime.Schemas;

using HtmlAgilityPack;

using Refit;

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

    async Task<Identifier[]> SearchBangumiIds(string searchStr) {
        int spaceIndex = searchStr.IndexOf(' ');
        if (spaceIndex != -1)
            searchStr = searchStr[..spaceIndex];

        HtmlDocument document = await SearchHtml(searchStr);
        
        var nodes = document.DocumentNode.SelectNodes("//ul[@class=\"list-inline an-ul\"]/li/a");
        if (nodes == null) return [];

        return nodes.Select(node => {
            string name = node.SelectSingleNode(".//div[@class=\"an-text\"]").InnerText.Trim().UnicodeUnescape();

            string path = node.Attributes["href"].Value;
            int id = int.Parse(path[(path.LastIndexOf('/') + 1)..]);

            return new Identifier(name, id);
        }).ToArray();
    }

    [Get("/RSS/Search")]
    Task<RssRoot> SearchRss(string searchStr);
}
