namespace WinLimit.Models;
public class ScheduleRule
{
    public int StartHour { get; set; }
    public int EndHour { get; set; }
    public int Duration { get { return EndHour - StartHour; } }
    public int CanvasTop { get { return StartHour * 20+30; } }
    public int CanvasHeight { get { return Duration * 20; } }

    public ScheduleRule(int startHour, int endHour)
    {
        StartHour = startHour;
        EndHour = endHour;
    }
}