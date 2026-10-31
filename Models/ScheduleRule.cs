using CommunityToolkit.Mvvm.ComponentModel;

namespace WinLimit.Models;

public partial class ScheduleRule : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Duration))]
    private int _startHour;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Duration))]
    private int _endHour;

    public int Duration => EndHour - StartHour;

    public ScheduleRule(int startHour, int endHour)
    {
        _startHour = startHour;
        _endHour = endHour;
    }
}
