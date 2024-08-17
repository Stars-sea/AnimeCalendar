using Microsoft.UI.Xaml.Media.Animation;

using System;

namespace AnimeCalendar.UI;

internal record NavigationInfo(
    Type    PageType, 
    string? Title, 
 // bool    Inherited, 
    object? Param,
    object? Tag,
    NavigationTransitionInfo? TransitionInfo = null
);
