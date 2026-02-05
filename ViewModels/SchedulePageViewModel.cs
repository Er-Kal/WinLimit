
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinLimit.Models;

namespace WinLimit.ViewModels;


public partial class SchedulePageViewModel : ViewModelBase
{
    private ScheduleService _scheduleService;
    [ObservableProperty]
    public Dictionary<string,WeekDay> _weekDays;
    public SchedulePageViewModel(ScheduleService scheduleService)
    {
        _weekDays = new Dictionary<string,WeekDay>();
        _scheduleService = scheduleService;
        _scheduleService.OnSchedulesChanged += SchedulesChanged;
        SchedulesChanged();
    }

    public void SchedulesChanged()
    {
        WeekDays = _scheduleService.WeekDays;
    }

    private int getY(int y)
    {
        return y*25;
    }

    [RelayCommand]
    private void AddRule()
    {
        ScheduleRuleWindow window = new ScheduleRuleWindow();
        window.DataContext = new ScheduleRuleWindowViewModel(_scheduleService, window);
        window.Show();
    }
    [RelayCommand]
    private void RemoveScheduleRule(ScheduleRule rule)
    {
        _scheduleService.RemoveRule(rule);
    }
}