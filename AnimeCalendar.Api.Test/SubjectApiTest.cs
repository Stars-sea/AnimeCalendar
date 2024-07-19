using AnimeCalendar.Api.Bangumi;

namespace AnimeCalendar.Api.Test;

[TestClass]
public class SubjectApiTest {
    ISubjectApi Api { get; init; }

    public SubjectApiTest() {
        Api = BgmApiServices.SubjectApi;
    }

    [TestMethod]
    [DataRow(397604, "ATRI -My Dear Moments-")]
    public async Task GetSubjectTest(int subjectId, string name) {
        var subject = await Api.GetSubject(subjectId);
        Assert.AreEqual(subject.Name, name);
    }

    [TestMethod]
    public async Task CalendarTest() {
        await Api.GetCalendar();
    }
}
