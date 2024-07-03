namespace AnimeCalendar.Api.Bangumi.Schemas;

public enum UserGroup: int {
    Admin           = 1,    // 管理员
    BangumiOp       = 2,    // Bangumi 管理猿
    DoujinOp        = 3,    // 天窗管理猿
    MutedUser       = 4,    // 禁言用户
    BannedUser      = 5,    // 禁止访问用户
    CharacterOp     = 8,    // 人物管理猿
    WikiOp          = 9,    // 维基条目管理猿
    User            = 10,   // 用户
    WikiMan         = 11,   // 维基人
}

public record User(
    Avatar Avatar,
    string Sign,
    string Username,
    string Nickname,
    int Id,
    UserGroup UserGroup
);
