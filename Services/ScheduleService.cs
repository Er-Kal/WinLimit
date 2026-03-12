using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WinLimit.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

public partial class ScheduleService : ObservableObject
{
    public Dictionary<string,WeekDay> WeekDays;
    public event Action? OnSchedulesChanged;
    private string[] days = ["Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"];
    private LocalStorageService _localStorageService;
    private APIService _apiService;
    public ScheduleService(LocalStorageService localStorageService, APIService apiService)
    {
        _localStorageService=localStorageService;
        _apiService = apiService;
        CreateNewDict();
        LoadSchedulesLocal();
        StartTrackingSchedules();
    }
    public void SchedulesChanged()
    {
        OnSchedulesChanged?.Invoke();
        SaveSchedules();
    }
    public void CreateNewDict()
    {
        WeekDays = new Dictionary<string, WeekDay>();
        foreach (string day in days)
        {
            WeekDays.Add(day, new WeekDay(day));
        }
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
    // Function to prepare for conversion to JSON to write to file and cloud 
    public void SaveSchedules()
    {
        Dictionary<string, List<ScheduleRule>> dict = new Dictionary<string, List<ScheduleRule>>();
        foreach (string day in days)
        {
            dict[day] = WeekDays[day].ToJSON();
        }
        JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(dict, jsonOptions);
        _localStorageService.SaveSchedules(jsonString);
        _apiService.updateProfileSchedule(jsonString);
    }
    // Loads schedules from local file
    public void LoadSchedulesLocal()
    {
        Dictionary<string, List<ScheduleRule>>? schedules = _localStorageService.LoadSchedules();
        LoadSchedules(schedules);
        OnSchedulesChanged?.Invoke();
    }
    public void LoadSchedules(Dictionary<string, List<ScheduleRule>>? schedules)
    {
        if (schedules == null) return;
        foreach (string day in days)
        {
            WeekDays[day].ScheduleRules.Clear();
            List<ScheduleRule> rules = schedules[day];
            foreach (ScheduleRule rule in rules)
            {
                WeekDays[day].AddRule(rule.StartHour, rule.EndHour);
            }
        }
        SaveSchedules();
    }
}