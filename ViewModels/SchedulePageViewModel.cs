
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;
using WinLimit.Models;
using WinLimit.Views;

namespace WinLimit.ViewModels;


public partial class SchedulePageViewModel : ViewModelBase
{
    private ScheduleService _scheduleService;
    [ObservableProperty]
    public Dictionary<string,WeekDay> _weekDays;
    [ObservableProperty]
    private ObservableCollection<ScheduleOption> _scheduleOptions = new();
    [ObservableProperty]
    private ScheduleOption? _selectedScheduleOption;
    private bool _refreshingOptions;
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
        RefreshScheduleOptions();
    }

    // Syncs the drop down with the available ScheduleConfigs (by display name) and the current one
    private void RefreshScheduleOptions()
    {
        var config = _scheduleService.ScheduleConfig;
        if (config == null) return;
        _refreshingOptions = true;
        try
        {
            var latest = config.Configs.Select(c => new ScheduleOption(c.Key, c.Value.Name)).ToList();
            latest.Add(ScheduleOption.NewConfig);
            if (!latest.SequenceEqual(ScheduleOptions))
            {
                ScheduleOptions.Clear();
                foreach (var option in latest) ScheduleOptions.Add(option);
            }
            SelectedScheduleOption = ScheduleOptions.FirstOrDefault(o => o.Id == config.CurrentConfig);
        }
        finally
        {
            _refreshingOptions = false;
        }
    }

    [RelayCommand]
    private void EditConfig()
    {
        var window = new EditScheduleConfigWindow();
        window.DataContext = new EditScheduleConfigWindowViewModel(_scheduleService, window);
        window.Show();
    }

    private void OpenNewConfigWindow()
    {
        var window = new NewScheduleConfigWindow();
        window.DataContext = new NewScheduleConfigWindowViewModel(_scheduleService, window);
        window.Show();
    }

    partial void OnSelectedScheduleOptionChanged(ScheduleOption? value)
    {
        if (_refreshingOptions || value == null) return;
        if (value.IsNewConfig)
        {
            // Revert the selection to the current config, then prompt for the new config's name
            Dispatcher.UIThread.Post(() =>
            {
                RefreshScheduleOptions();
                OpenNewConfigWindow();
            });
            return;
        }
        _scheduleService.SwitchConfig(value.Id);
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
