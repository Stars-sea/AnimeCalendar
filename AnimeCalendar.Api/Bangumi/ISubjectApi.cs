﻿using AnimeCalendar.Api.Bangumi.Schemas;

using Refit;

using System.Threading.Tasks;

namespace AnimeCalendar.Api.Bangumi;

[Headers(BangumiApp.USER_AGENT)]
public interface ISubjectApi {
    [Get("/calendar")]
    Task<Calendar[]> GetCalendar();

    [Get("/v0/subjects/{id}")]
    Task<Subject> GetSubject(int id);

    [Get("/v0/subjects/{id}/persons")]
    Task<Person[]> GetPersons(int id);

    [Get("/v0/subjects/{id}/characters")]
    Task<Character[]> GetCharacters(int id);

    [Get("/v0/subjects/{id}/subjects")]
    Task<RelatedSubject[]> GetRelatedSubjects(int id);

    // TODO: Subject Search
}
