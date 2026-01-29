using System.Collections.Generic;
namespace WinLimit.Models;

public class WeekDay
{
    public string WeekDayName{get;set;}
    public List<ScheduleRule> ScheduleRules{get;set;}
    public WeekDay(string weekDayName, List<ScheduleRule> scheduleRules)
    {
        ScheduleRules = scheduleRules;
        WeekDayName = weekDayName;
    }
    public WeekDay(string weekDayName)
    {
        WeekDayName=weekDayName;
        ScheduleRules=new List<ScheduleRule>();
        ScheduleRules.Add(new ScheduleRule(12,15));
    }
}