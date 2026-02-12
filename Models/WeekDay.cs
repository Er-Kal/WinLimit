using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tmds.DBus.Protocol;
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
    public List<ScheduleRule> ToJSON()
    {
        /*List<ScheduleRule> rules = new List<ScheduleRule>();
        foreach (ScheduleRule rule in ScheduleRules)
        {
            rules.Add( new
            {
                startHour=rule.StartHour,
                endHour=rule.EndHour,
            });
        }
        return rules;*/
        return ScheduleRules.ToList<ScheduleRule>();
    }
}