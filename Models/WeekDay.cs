using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace WinLimit.Models;

public class WeekDay
{
    public string WeekDayName{get;set;}
    public ObservableCollection<ScheduleRule> ScheduleRules{get;set;}
    public WeekDay(string weekDayName, ObservableCollection<ScheduleRule> scheduleRules)
    {
        ScheduleRules = scheduleRules;
        WeekDayName = weekDayName;
    }
    public WeekDay(string weekDayName)
    {
        WeekDayName=weekDayName;
        ScheduleRules=new ObservableCollection<ScheduleRule>();
    }
    public void AddRule(int startTime, int endTime)
    {
        ScheduleRules.Add(new ScheduleRule(startTime, endTime));
        
    }
}