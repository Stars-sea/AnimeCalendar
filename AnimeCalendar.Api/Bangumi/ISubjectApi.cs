using AnimeCalendar.Api.Bangumi.Schemas;

using Refit;

using System.Threading.Tasks;

namespace AnimeCalendar.Api.Bangumi;

[Headers(
    "Authorization: Bearer",
    "User-Agent: Stras-sea/AnimeCalendar (https://github.com/Stars-sea/AnimeCalendar)"
)]
public interface ISubjectApi {
    [Get("/v0/subjects/{id}")]
    Task<Subject> GetSubject(int id);

    [Get("/calendar")]
    Task<Calendar[]> GetCalendar();
}
