﻿using AnimeCalendar.Api.Bangumi.Auth;
using AnimeCalendar.Api.Converter;
using AnimeCalendar.Api.Storage;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Refit;

namespace AnimeCalendar.Api.Bangumi;

public static class BgmApiServices {
    public static readonly ISubjectApi    SubjectApi;
    public static readonly IUserApi       UserApi;
    public static readonly ICollectionApi CollectionApi;

    public const string BASE_ADDRESS = "https://api.bgm.tv";

    public static readonly RefitSettings ServiceSettings;

    public static readonly JsonSerializerSettings SerializerSettings = new() {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new DefaultContractResolver() {
            NamingStrategy = new SnakeCaseNamingStrategy()
        },
        Converters = [ new DateOnlyConverter(), new TimeSpanConverter() ]
    };

    static BgmApiServices() {
        ServiceSettings = new(
            new NewtonsoftJsonContentSerializer(SerializerSettings),
            new EnumUrlParameterFormatter()
        );

        HttpClient client = new(new AuthHeaderHandler()) {
            BaseAddress = new Uri(BASE_ADDRESS)
        };

        SubjectApi      = RestService.For<ISubjectApi>(client, ServiceSettings);
        UserApi         = RestService.For<IUserApi>(client, ServiceSettings);
        CollectionApi   = RestService.For<ICollectionApi>(client, ServiceSettings);
    }

    public static void UpdateTokenStorage(IAuthTokenStorage? tokenStorage) {
        AuthHeaderHandler.TokenStorage = tokenStorage;
    }
}
