using WinLimit.Models;

public class ScheduleDefinition
{
    public string Name { get; set; }
    public WeekDay[] WeekDays { get; set; }

    public ScheduleDefinition(string name, WeekDay[] weekDays)
    {
        Name = name;
        WeekDays = weekDays;
    }
}