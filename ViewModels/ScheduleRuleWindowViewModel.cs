using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinLimit.Models;

namespace WinLimit.ViewModels;

public partial class ScheduleRuleWindowViewModel : INotifyPropertyChanged
{
    private int _lowerValue;
    private int _upperValue;
    public int LowerValue
    {
        get { return _lowerValue; }
        set
        {
            _lowerValue = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TimeString));
        }
    }
    public int UpperValue
    {
        get { return _upperValue; }
        set
        {
            _upperValue = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TimeString));
        }
    }
    private string _selectedDay = "Monday";
    public string SelectedDay
    {
        get => _selectedDay;
        set => _selectedDay = value;
    }
    public List<string> DaysOfWeek { get; } = new()
    {
        "Monday", "Tuesday", "Wednesday",
        "Thursday", "Friday", "Saturday", "Sunday"
    };
    public string TimeString => $"{LowerValue}:00 to {UpperValue}:00";
    public string ErrorMessage
    {
        get { return _errorMessage; }
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }
    public string _errorMessage;
    ScheduleService _scheduleService;
    ScheduleRuleWindow _window;
    public event PropertyChangedEventHandler PropertyChanged;
    public ScheduleRuleWindowViewModel(ScheduleService scheduleService, ScheduleRuleWindow window)
    {
        _scheduleService = scheduleService;
        _window = window;
    }
    [RelayCommand]
    public void SubmitRule()
    {
        if (UpperValue == LowerValue)
        {
            ErrorMessage = "You must have a duration greater than 0";
            return;
        }
        WeekDay day = _scheduleService.WeekDays[SelectedDay];
        ObservableCollection<ScheduleRule> existingRules = day.ScheduleRules;
        bool toBeAdded = true;

        foreach (ScheduleRule rule in existingRules)
        {
            if (LowerValue >= rule.StartHour && LowerValue < rule.EndHour)
                toBeAdded = false;
            if (UpperValue > rule.StartHour && UpperValue <= rule.EndHour)
                toBeAdded = false;
        }

        if (!toBeAdded)
        {
            ErrorMessage = "This rule overlaps with an existing rule";
            return;
        }

        day.AddRule(LowerValue, UpperValue);
        _scheduleService.WeekDays[SelectedDay] = day;
        _scheduleService.SchedulesChanged();
        _window.Close();
    }
    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}