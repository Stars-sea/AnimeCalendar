using AnimeCalendar.Api.Mikanime;
using AnimeCalendar.Api.Mikanime.Rss;

using HtmlAgilityPack;

namespace AnimeCalendar.Api.Test;

[TestClass]
public class SearchAnimeTest {
    [TestMethod]
    public async Task RssTest() {
        RssRoot rss = await MikanimeServices.SearchAnime.SearchRss("亚托莉 -我挚爱的时光-");
        Assert.IsNotNull(rss);
    }

    [TestMethod]
    public async Task HtmlTest() {
        var paths = await MikanimeServices.SearchAnime.SearchDetailPagePaths("亚托莉 -我挚爱的时光-");
        Assert.AreEqual(paths.Values[0], "/Home/Bangumi/3386");
    }
}
