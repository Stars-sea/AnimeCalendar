using AnimeCalendar.Api.Bangumi;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Refit;

using System.Diagnostics;

namespace AnimeCalendar.Api.Test;

[TestClass]
public class SubjectApiTest {
    ISubjectApi Api { get; init; }

    public SubjectApiTest() {
        Api = BgmApiServices.SubjectApi;
    }

    [TestMethod]
    public void GetSubjectTest() {  // 奇怪, 为什么非要同步方法...
        var subject = Api.GetSubject(397604).GetAwaiter().GetResult();
        Assert.AreEqual(subject.Name, "ATRI -My Dear Moments-");
    }

    [TestMethod]
    public async Task CalendarTest() {
        await Api.GetCalendar();
    }
}
