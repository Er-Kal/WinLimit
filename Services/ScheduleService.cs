using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WinLimit.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

public partial class ScheduleService : ObservableObject
{
    public ScheduleConfig ScheduleConfig { get; set; }
    public Dictionary<string,WeekDay> WeekDays = new Dictionary<string, WeekDay>();
    public event Action? OnSchedulesChanged;
    private string[] days = new string[] {"Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"};
    private LocalStorageService _localStorageService;
    private APIService _apiService;
    public ScheduleService(LocalStorageService localStorageService, APIService apiService)
    {
        _localStorageService=localStorageService;
        _apiService = apiService;
        LoadSchedulesLocal();
        StartTrackingSchedules();
    }
    public void SchedulesChanged()
    {
        OnSchedulesChanged?.Invoke();
        SaveSchedules();
    }
    public bool IsScheduledBlocked()
    {
        string dayOfWeek = DateTime.Now.DayOfWeek.ToString();
        WeekDay currentDay = WeekDays[dayOfWeek];
        int currentHour = DateTime.Now.Hour;
        foreach (ScheduleRule rule in currentDay.ScheduleRules)
        {
            if (rule.StartHour <= currentHour && currentHour < rule.EndHour)
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
        /*foreach (string day in days)
        {
            dict[day] = WeekDays[day].ToJSON();
        }*/

        JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(ScheduleConfig, jsonOptions);
        _localStorageService.SaveSchedules(jsonString);
        _apiService.updateProfileSchedule(jsonString);
    }
    // Loads schedules from local file
    public void LoadSchedulesLocal()
    {
        ScheduleConfig schedules = _localStorageService.LoadSchedules();
        LoadSchedules(schedules);
        OnSchedulesChanged?.Invoke();
    }
    public void LoadSchedules(ScheduleConfig schedules)
    {
        if (schedules == null)
        {
            ScheduleConfig = new ScheduleConfig(new Dictionary<Guid, ScheduleDefinition>());
            WeekDay[] days = new WeekDay[7];
            for (int i = 0; i<7; i++)
            {
                days[i] = new WeekDay(((DayOfWeek)i).ToString());
            }
            ScheduleDefinition defaultDefinition = new ScheduleDefinition("Default", days);
            Guid defGuid = Guid.NewGuid();
            ScheduleConfig.Configs.Add(defGuid, defaultDefinition);
            ScheduleConfig.CurrentConfig = defGuid;
        }
        else
        {
            ScheduleConfig = schedules;
        }
        RebuildWeekDays();
        SaveSchedules();
    }
    // Rebuilds the WeekDays lookup from the currently selected config
    private void RebuildWeekDays()
    {
        var newWeekDays = new Dictionary<string, WeekDay>();
        foreach (WeekDay day in ScheduleConfig.Configs[ScheduleConfig.CurrentConfig].WeekDays)
        {
            newWeekDays[day.WeekDayName] = day;
        }
        WeekDays = newWeekDays;
    }
    public bool ConfigNameExists(string name, Guid? excludeId = null)
    {
        foreach (var entry in ScheduleConfig.Configs)
        {
            if (entry.Key == excludeId) continue;
            if (string.Equals(entry.Value.Name, name, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }
    // Adds a new empty schedule config with the given display name, makes it active and saves
    public Guid AddConfig(string name)
    {
        WeekDay[] newDays = new WeekDay[7];
        for (int i = 0; i < 7; i++)
        {
            newDays[i] = new WeekDay(((DayOfWeek)i).ToString());
        }
        Guid id = Guid.NewGuid();
        ScheduleConfig.Configs.Add(id, new ScheduleDefinition(name, newDays));
        ScheduleConfig.CurrentConfig = id;
        RebuildWeekDays();
        SchedulesChanged();
        return id;
    }
    public void RenameConfig(Guid id, string newName)
    {
        if (!ScheduleConfig.Configs.TryGetValue(id, out var definition)) return;
        definition.Name = newName;
        SchedulesChanged();
    }
    // Deletes a config (the last remaining one can't be deleted); switches to another if it was active
    public bool DeleteConfig(Guid id)
    {
        if (ScheduleConfig.Configs.Count <= 1 || !ScheduleConfig.Configs.ContainsKey(id)) return false;
        ScheduleConfig.Configs.Remove(id);
        if (ScheduleConfig.CurrentConfig == id)
        {
            foreach (Guid remaining in ScheduleConfig.Configs.Keys)
            {
                ScheduleConfig.CurrentConfig = remaining;
                break;
            }
        }
        RebuildWeekDays();
        SchedulesChanged();
        return true;
    }
    // Switches the active schedule config and notifies listeners
    public void SwitchConfig(Guid id)
    {
        if (ScheduleConfig.CurrentConfig == id || !ScheduleConfig.Configs.ContainsKey(id)) return;
        ScheduleConfig.CurrentConfig = id;
        RebuildWeekDays();
        SchedulesChanged();
    }
}