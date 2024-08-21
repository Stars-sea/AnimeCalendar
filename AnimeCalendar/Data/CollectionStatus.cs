using AnimeCalendar.Api.Bangumi.Schemas;

namespace AnimeCalendar.Data;

/// <summary>
/// <para>ComboBox Item</para>
/// <see cref="Controls.Components.CollectionStatusSelector"/>
/// </summary>
public record CollectionStatus(
    string          Name,
    CollectionType  Type
);
