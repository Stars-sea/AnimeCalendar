using Refit;

namespace AnimeCalendar.Api.Mikanime;

public class MikanimeServices {
    public static readonly ISearchAnime SearchAnime;

    public static readonly RefitSettings ServiceSettings;

    public const string BASE_ADDRESS = "https://mikanime.tv";

    static MikanimeServices() {
        ServiceSettings = new(new XmlContentSerializer());

        SearchAnime = RestService.For<ISearchAnime>(BASE_ADDRESS, ServiceSettings);
    }
}
