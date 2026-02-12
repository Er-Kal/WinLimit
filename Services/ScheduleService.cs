using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WinLimit.Models;

public partial class ScheduleService : ObservableObject
{
    public Dictionary<string,WeekDay> WeekDays;
    public event Action? OnSchedulesChanged;
    private string[] days = ["Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"];
    private LocalStorageService _localStorageService;
    public ScheduleService(LocalStorageService localStorageService)
    {
        WeekDays = new Dictionary<string, WeekDay>();
        foreach (string day in days)
        {
            WeekDays.Add(day,new WeekDay(day));
        }
        StartTrackingSchedules();
        _localStorageService=localStorageService;
        LoadSchedules();
    }
    public void SchedulesChanged()
    {
        OnSchedulesChanged?.Invoke();
        SaveSchedules();
    }
    public bool IsScheduledBlocked()
    {
        WeekDay currentDay = WeekDays[DateTime.Now.DayOfWeek.ToString()];
        int currentHour = DateTime.Now.Hour;
        foreach (ScheduleRule rule in currentDay.ScheduleRules)
        {
            if (rule.StartHour <= currentHour && currentHour <= rule.EndHour)
                return true;
        }
        return false;
    }
    private async void StartTrackingSchedules()
    {
        while (true)
        {
            if (IsScheduledBlocked())
            {
                SchedulesChanged();
            }
            await Task.Delay(30000);
        }
    }
    public void RemoveRule(ScheduleRule rule)
    {
        foreach (string day in days)
        {
            if (WeekDays[day].ScheduleRules.Contains(rule))
            {
                WeekDays[day].ScheduleRules.Remove(rule);
            }
        }
        SchedulesChanged();
    }
    // Function to prepare for conversion to JSON to write to file
    public void SaveSchedules()
    {
        Dictionary<string, List<ScheduleRule>> dict = new Dictionary<string, List<ScheduleRule>>();
        foreach (string day in days)
        {
            dict[day] = WeekDays[day].ToJSON();
        }
        _localStorageService.SaveSchedules(dict);
    }
    public void LoadSchedules()
    {
        Dictionary<string, List<ScheduleRule>>? schedules = _localStorageService.LoadSchedules();
        if (schedules == null) return;
        foreach (string day in days)
        {
            List<ScheduleRule> rules = schedules[day];
            foreach (ScheduleRule rule in rules)
            {
                WeekDays[day].AddRule(rule.StartHour, rule.EndHour);
            }
        }

        SchedulesChanged();
    }
}