using AnimeCalendar.Api.Bangumi;
using AnimeCalendar.Api.Mikanime;

namespace AnimeCalendar.Api.Test.Mikanime;

[TestClass]
public class BangumiApiTest {
    private static IBangumiApi Api => MikanimeServices.BangumiApi;

    [TestMethod]
    [DataRow(3386)]
    public async Task BgmNameTest(int bangumiId) {
        BangumiPage page = await Api.BangumiPage(bangumiId);
        int? subjectId = page.GetBgmSubjectId();
        Assert.IsNotNull(subjectId);

        var bgmSubject = await BgmApiServices.SubjectApi.GetSubject((int)subjectId);
        Assert.AreEqual(bgmSubject.NameCn.Trim(), page.GetBangumiName());
    }

    [TestMethod]
    [DataRow(3386)]
    public async Task SubgroupTest(int bangumiId) {
        BangumiPage page = await Api.BangumiPage(bangumiId);
        var subgroups = page.GetSubgroups();
        Assert.IsNotNull(subgroups);
    }
}
