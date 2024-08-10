using Refit;

namespace AnimeCalendar.Api.Converter;

internal class LowerUrlParameterKeyFormatter : DefaultUrlParameterKeyFormatter {
    public override string Format(string key) => base.Format(key).ToLower();
}
