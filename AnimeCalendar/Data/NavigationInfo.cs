using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

using System;

namespace AnimeCalendar.Data;

internal record NavigationInfo(
    Type    PageType, 
    string? Title, 
 // bool    Inherited, 
    object? Param,
    NavigationTransitionInfo? TransitionInfo = null
);
