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


public class ModifyCollectionRequest {
    public CollectionType? Type      { get; init; }
    public int?            Rate      { get; init; }
    public int?            VolStatus { get; init; }
    public string?         Comment   { get; init; }
    public bool?           Private   { get; init; }
    public Tag[]?          Tags      { get; init; }

    [Obsolete("不能修改剧集条目的完成度")]
    public int? EpStatus { get; set; }
}
