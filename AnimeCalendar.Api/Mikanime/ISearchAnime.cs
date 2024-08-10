using AnimeCalendar.Api.Mikanime.Rss;

using HtmlAgilityPack;

using Refit;

using System.Collections.Frozen;

namespace AnimeCalendar.Api.Mikanime;

public interface ISearchAnime {
    [Get("/Home/Search")]
    Task<string> SearchRawHtml(string searchstr);

    async Task<HtmlDocument> SearchHtml(string searchstr) {
        string raw = await SearchRawHtml(searchstr);
        HtmlDocument document = new();
        document.LoadHtml(raw);
        return document;
    }

    async Task<FrozenDictionary<string, string>> SearchDetailPagePaths(string searchstr) {
        HtmlDocument document = await SearchHtml(searchstr);
        HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//ul[@class=\"list-inline an-ul\"]/li/a"); /* //*[@id="sk-container"]/div[2]/ul/li/a */
        return nodes.ToFrozenDictionary(
            node => node.SelectSingleNode("//div[@class=\"an-text\"]").InnerText,
            node => node.Attributes["href"].Value
        );
    }

    [Get("/RSS/Search")]
    Task<RssRoot> SearchRss(string searchstr);
}
