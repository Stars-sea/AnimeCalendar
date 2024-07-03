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
        JsonSerializerSettings jsonSerializer = new() {
            ContractResolver = new DefaultContractResolver() {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        RefitSettings settings = new(new NewtonsoftJsonContentSerializer(jsonSerializer));

        Api = RestService.For<ISubjectApi>("https://api.bgm.tv", settings);
    }

    [TestMethod]
    public void GetSubjectNetworkTest() {
        string url = "https://api.bgm.tv/v0/subjects/397604";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("User-Agent", "Stras-sea/AnimeCalendar (https://github.com/Stars-sea/AnimeCalendar)");
        client.GetStringAsync(url).GetAwaiter().GetResult();
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