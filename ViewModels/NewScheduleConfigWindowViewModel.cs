using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WinLimit.ViewModels;

public partial class NewScheduleConfigWindowViewModel : ViewModelBase
{
    private readonly ScheduleService _scheduleService;
    private readonly Window _window;

    [ObservableProperty]
    private string _displayName = "";
    [ObservableProperty]
    private string _errorMessage = "";

    public NewScheduleConfigWindowViewModel(ScheduleService scheduleService, Window window)
    {
        _scheduleService = scheduleService;
        _window = window;
    }

    [RelayCommand]
    private void Submit()
    {
        string name = DisplayName.Trim();
        if (name.Length == 0)
        {
            ErrorMessage = "Please enter a name";
            return;
        }
        if (_scheduleService.ConfigNameExists(name))
        {
            ErrorMessage = "A schedule with this name already exists";
            return;
        }
        _scheduleService.AddConfig(name);
        _window.Close();
    }
}
