namespace WinLimit.Models;
public class ScheduleRule
{
    public int StartHour { get; set; }
    public int EndHour { get; set; }
    public int Duration => EndHour - StartHour;

    public ScheduleRule(int startHour, int endHour)
    {
        StartHour = startHour;
        EndHour = endHour;
    }
}