﻿using Refit;

using System.Reflection;

namespace AnimeCalendar.Api.Bangumi;

internal class UrlParameterFormatter : DefaultUrlParameterFormatter {
    public override string? Format(object? parameterValue, ICustomAttributeProvider attributeProvider, Type type)
        => parameterValue != null && parameterValue.GetType().IsEnum
            ? ((int)parameterValue).ToString()
            : base.Format(parameterValue, attributeProvider, type);
}
