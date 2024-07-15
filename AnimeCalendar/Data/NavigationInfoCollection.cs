using System.Collections.ObjectModel;

namespace AnimeCalendar.Data;

internal class NavigationInfoCollection : Collection<NavigationInfo> {

    public int Index { get; private set; } = -1;

    public NavigationInfo Current => this[Index];

    public bool CanGoForward => Index < Count - 1;
    public bool CanGoBack    => Index > 0;

    public NavigationInfo? GoForward() {
        if (Count <= 1 || Index == Count - 1) return null;
        return this[++Index];
    }

    public NavigationInfo? GoBack() {
        if (Count <= 1 || Index == 0) return null;
        return this[--Index];
    }

    public void Navigate(NavigationInfo info) {
        if (0 <= Index && Index < Count && info == Current) return;

        if (CanGoForward) {
            for (int i = Count - 1; Index < i; i--) {
                RemoveAt(i);
            }
        }

        Add(info);
    }

    protected new void Add(NavigationInfo info) {
        base.Add(info);
        Index++;
    }
}