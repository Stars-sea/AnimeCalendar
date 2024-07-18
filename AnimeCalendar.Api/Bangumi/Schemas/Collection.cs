using Newtonsoft.Json;

namespace AnimeCalendar.Api.Bangumi.Schemas;

public enum CollectionType : int {
    Wish    = 1, // 想看
    Collect = 2, // 看过
    Do      = 3, // 在看
    OnHold  = 4, // 搁置
    Dropped = 5, // 抛弃
}

public record UserCollection(
    DateTime        UpdatedAt,
    string?         Comment,
    string[]        Tags,
    SlimSubject     Subject,
    int             SubjectId,
    int             VolStatus,
    int             EpStatus,
    SubjectType     SubjectType,
    CollectionType  Type,
    int             Rate,
    bool            Private
);


public record ModifyCollectionRequest(
    CollectionType? Type,
    int?            Rate,
    int?            VolStatus,
    string?         Comment,
    bool?           Private,
    Tag[]?          Tags
) {
    [Obsolete("不能修改剧集条目的完成度")]
    public int? EpStatus { get; set; }
}
