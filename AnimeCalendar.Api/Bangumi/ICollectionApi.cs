using AnimeCalendar.Api.Bangumi.Schemas;

using Refit;

namespace AnimeCalendar.Api.Bangumi;

[Headers(ModuleSettings.USER_AGENT)]
public interface ICollectionApi {
    [Get("/v0/users/{username}/collections")]
    Task<PagedObject<UserCollection>> GetCollections(
        string username,
        int offset = 0,
        int limit  = 30,
        CollectionType type = CollectionType.Do,
        SubjectType subjectType = SubjectType.Anime
    );

    [Get("/v0/users/{username}/collections/{subject_id}")]
    Task<UserCollection> GetCollection(
        string username,
        int    subject_id
    );

    /* 不能修改剧集条目的完成度 */
    [Post("/v0/users/-/collections/{subject_id}")]
    Task PostCollection(
        int subject_id, 
        [Body] ModifyCollectionRequest _
    );

    /* 不能修改剧集条目的完成度 */
    [Patch("/v0/users/-/collections/{subject_id}")]
    Task PatchCollection(
        int subject_id,
        [Body] ModifyCollectionRequest _
    );

    [Get("/v0/users/-/collections/{subject_id}/episodes")]
    Task<PagedObject<EpCollection>> GetEpisodes(
        int subject_id,
        int offset = 0,
        int limit  = 30,
        EpType? episodeType = null
    );

    [Patch("/v0/users/-/collections/{subject_id}/episodes")]
    Task PatchEpisodes(int subject_id, [Body] ModifyEpRequest _);

    [Get("/v0/users/-/collections/-/episodes/{episode_id}")]
    Task<EpCollection> GetEpisode(int episode_id);

    [Headers("Content-Type: application/json")]
    [Put("/v0/users/-/collections/-/episodes/{episode_id}")]
    Task PutEpCollectionType(int episode_id, [Body] ModifyEpRequest _);
}
