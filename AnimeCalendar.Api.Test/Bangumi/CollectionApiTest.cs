using AnimeCalendar.Api.Bangumi;

namespace AnimeCalendar.Api.Test.Bangumi;

[TestClass]
public class CollectionApiTest {
    private static string Username => ApiTestSetup.User.Username;
    private static ICollectionApi Api => BgmApiServices.CollectionApi;

    [TestMethod]
    public async Task GetCollectionsTest() {
        await Api.GetCollections(Username);
    }

    [TestMethod]
    [DataRow("stars_sea", 424883, "時々ボソッとロシア語でデレる隣のアーリャさん")]
    public async Task GetCollectionTest(string username, int subjectId, string subjectName) {
        var collection = await Api.GetCollection(username, subjectId);
        Assert.AreEqual(subjectName, collection.Subject.Name);
    }
}
