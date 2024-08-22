using AnimeCalendar.Api.Bangumi.Schemas;

using System.Linq;

namespace AnimeCalendar.Data;

/// <summary>
/// <para>Wrapper of <see cref="CollectionType"/></para>
/// </summary>
public record CollectionStatus(
    string          Name,
    CollectionType  Type
) {
    public static readonly CollectionStatus[] Statuses = [
        new("在看", CollectionType.Do),
        new("想看", CollectionType.Wish),
        new("看过", CollectionType.Collect),
        new("搁置", CollectionType.OnHold),
        new("抛弃", CollectionType.Dropped)
    ];

    public static CollectionStatus Wrap(CollectionType type)
        => Statuses.First(s => s.Type == type);
}
