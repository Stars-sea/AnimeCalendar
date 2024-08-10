using AnimeCalendar.Api.Converter;

using Refit;

namespace AnimeCalendar.Api.Mikanime;

public class MikanimeServices {
    public static readonly ISearchAnimeApi  SearchAnimeApi;
    public static readonly IBangumiApi      BangumiApi;

    public static readonly RefitSettings ServiceSettings;

    public const string BASE_ADDRESS = "https://mikanime.tv";

    static MikanimeServices() {
        ServiceSettings = new(
            new XmlContentSerializer(),
            urlParameterKeyFormatter: new LowerUrlParameterKeyFormatter()
        );

        SearchAnimeApi  = RestService.For<ISearchAnimeApi>(BASE_ADDRESS, ServiceSettings);
        BangumiApi      = RestService.For<IBangumiApi>(BASE_ADDRESS, ServiceSettings);
    }
}
