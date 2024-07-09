using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Refit;
using AnimeCalendar.Api.Bangumi.Auth;
using AnimeCalendar.Api.Storage;

namespace AnimeCalendar.Api.Bangumi;

public static class BangumiApiServices {
    public static ISubjectApi SubjectApi    { get; private set; }
    public static IUserApi    UserApi       { get; private set; }

    public const string BASE_ADDRESS = "https://api.bgm.tv";

    public static readonly JsonSerializerSettings SerializerSettings = new() {
        ContractResolver = new DefaultContractResolver() {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    public static void Init(IAuthTokenStorage tokenStorage) {
        RefitSettings settings = new(new NewtonsoftJsonContentSerializer(SerializerSettings));
        HttpClient client = new(new AuthHeaderHandler(tokenStorage)) {
            BaseAddress = new Uri(BASE_ADDRESS)
        };
        
        SubjectApi  = RestService.For<ISubjectApi>(client, settings);
        UserApi     = RestService.For<IUserApi>(client, settings);
    }
}
