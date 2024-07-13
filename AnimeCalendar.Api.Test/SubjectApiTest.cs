using AnimeCalendar.Api.Bangumi;

namespace AnimeCalendar.Api.Test;

[TestClass]
public class SubjectApiTest {
    ISubjectApi Api { get; init; }

    public SubjectApiTest() {
        Api = BgmApiServices.SubjectApi;
    }

    [TestMethod]
    public async Task GetSubjectTest() {
        var subject = await Api.GetSubject(397604);
        Assert.AreEqual(subject.Name, "ATRI -My Dear Moments-");
    }

    [TestMethod]
    public async Task CalendarTest() {
        await Api.GetCalendar();
    }
}
