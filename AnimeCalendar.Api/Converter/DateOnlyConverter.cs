using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AnimeCalendar.Api.Converter;

internal class DateOnlyConverter : IsoDateTimeConverter {
    public const string FORMAT = "yyyy-MM-dd";
    
    public DateOnlyConverter() {
        DateTimeFormat = FORMAT;
    }
}
