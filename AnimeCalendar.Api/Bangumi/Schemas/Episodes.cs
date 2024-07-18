namespace AnimeCalendar.Api.Bangumi.Schemas;

public enum EpType : int {
    Feature = 0, // 本篇
    SP      = 1, // SP
    OP      = 2, // OP
    ED      = 3, // ED
}

public enum EpCollectionType : int {
    NoCollect   = 0, // 未收藏
    Wish        = 1, // 想看
    Collect     = 2, // 看过
    Dropped     = 3, // 抛弃
}

public record Episode(
    DateOnly    AirDate,
    string      Name,
    string      NameCn,
    TimeSpan    Duration,
    string      Desc,
    int         Ep,
    int         Sort,
    int         Id,
    int         SubjectId,
    int         Comment,
    EpType      Type,
    int         Disc, // 音乐曲目的碟片数
    int         DurationSeconds
);

public record EpCollection(
    Episode Episode,
    EpCollectionType Type
);

public record ModifyEpRequest(
    int[]? EpisodeId,
    EpCollectionType Type
);
