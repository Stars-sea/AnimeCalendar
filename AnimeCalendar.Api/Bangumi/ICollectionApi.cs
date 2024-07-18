using AnimeCalendar.Api.Bangumi.Schemas;

using Refit;

namespace AnimeCalendar.Api.Bangumi;

[Headers(BangumiApp.USER_AGENT)]
public interface ICollectionApi {
    [Get("/v0/users/{username}/collections")]
    Task<PagedObject<UserCollection>> GetCollections(
        string username,
        int offset = 0,
        int limit  = 30,
        CollectionType type = CollectionType.Do,
        SubjectType subjectType = SubjectType.Anime
    );

    [Get("/v0/users/{username}/collections/{subjectId}")]
    Task<UserCollection> GetCollection(
        string username,
        int    subjectId
    );

    /* 不能修改剧集条目的完成度 */
    [Post("/v0/users/-/collections/{subjectId}")]
    Task PostCollection(
        int subjectId, 
        [Body] ModifyCollectionRequest _
    );

    /* 不能修改剧集条目的完成度 */
    [Patch("/v0/users/-/collections/{subjectId}")]
    Task PatchCollection(
        int subjectId,
        [Body] ModifyCollectionRequest _
    );

    [Get("/v0/users/-/collections/{subjectId}/episodes")]
    Task<PagedObject<EpCollection>> GetEpisodes(
        int subjectId,
        int offset = 0,
        int limit  = 30,
        EpType? episodeType = null
    );

    [Patch("/v0/users/-/collections/{subjectId}/episodes")]
    Task PatchEpisodes(int subjectId, [Body] ModifyEpRequest _);

    [Get("/v0/users/-/collections/-/episodes/{episodeId}")]
    Task<EpCollection> GetEpisode(int episodeId);

    [Put("/v0/users/-/collections/-/episodes/{episodeId}")]
    Task PutEpCollectionType([Body] ModifyEpRequest _);
}
