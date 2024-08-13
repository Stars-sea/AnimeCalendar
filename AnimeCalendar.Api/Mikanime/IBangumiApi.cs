using AnimeCalendar.Api.Mikanime.Schemas;

using HtmlAgilityPack;

using Refit;

namespace AnimeCalendar.Api.Mikanime;

[Headers(ModuleSettings.USER_AGENT)]
public interface IBangumiApi {
    [Get("/Home/Bangumi/{bangumiId}")]
    Task<string> BangumiRawHtml(int bangumiId);

    async Task<HtmlDocument> BangumiHtml(int bangumiId) {
        string raw = await BangumiRawHtml(bangumiId);
        HtmlDocument document = new();
        document.LoadHtml(raw);
        return document;
    }

    async Task<BangumiPage> BangumiPage(int bangumiId) {
        HtmlDocument document = await BangumiHtml(bangumiId);
        return new BangumiPage(document);
    }

    [Get("/RSS/Bangumi")]
    Task<RssRoot> BangumiRss(int bangumiId);

    [Get("/RSS/Bangumi")]
    Task<RssRoot> BangumiRss(int bangumiId, int subgroupId);
}
