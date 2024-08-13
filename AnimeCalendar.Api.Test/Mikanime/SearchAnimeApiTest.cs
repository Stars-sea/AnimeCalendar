using AnimeCalendar.Api.Mikanime;
using AnimeCalendar.Api.Mikanime.Schemas;

namespace AnimeCalendar.Api.Test.Mikanime;

[TestClass]
public class SearchAnimeApiTest {
    private static ISearchAnimeApi Api => MikanimeServices.SearchAnimeApi;

    [TestMethod]
    [DataRow("亚托莉 -我挚爱的时光-")]
    public async Task RssTest(string searchStr) {
        RssRoot rss = await Api.SearchRss(searchStr);
        Assert.IsNotNull(rss);
    }

    [TestMethod]
    [DataRow("亚托莉 -我挚爱的时光-", (int[])[ 3386 ])]
    public async Task HtmlTest(string searchStr, int[] bangumiIds) {
        var ids = (await Api.SearchBangumiIds(searchStr)).Select(id => id.Id);
        Assert.IsTrue(ids.Count() == bangumiIds.Length);
        Assert.IsTrue(ids.All(i => bangumiIds.Contains(i)));
    }
}
