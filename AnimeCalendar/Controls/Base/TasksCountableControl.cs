using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AnimeCalendar.Controls.Base;

public class TasksCountableControl : UserControl {
    public int RunningTasksCount {
        get { return (int)GetValue(RunningTasksCountProperty); }
        set { SetValue(RunningTasksCountProperty, value); }
    }

    public static readonly DependencyProperty RunningTasksCountProperty =
        DependencyProperty.Register("RunningTasksCount", typeof(int),
            typeof(WatchProgressControl), new PropertyMetadata(0));
}