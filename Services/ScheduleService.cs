using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WinLimit.Models;

public partial class ScheduleService : ObservableObject
{
    public Dictionary<string,WeekDay> WeekDays;
    public event Action? OnSchedulesChanged;
    public ScheduleService()
    {
        WeekDays = new Dictionary<string, WeekDay>();
        string[] days = ["Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"];
        foreach (string day in days)
        {
            WeekDays.Add(day,new WeekDay(day));
        }
        StartTrackingSchedules();
    }
    public void SchedulesChanged()
    {
        OnSchedulesChanged?.Invoke();
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
}