using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WinLimit.ViewModels;

public partial class EditScheduleConfigWindowViewModel : ViewModelBase
{
    private readonly ScheduleService _scheduleService;
    private readonly Window _window;
    private readonly Guid _configId;

    [ObservableProperty]
    private bool _isChoosing = true;
    [ObservableProperty]
    private bool _isRenaming;
    [ObservableProperty]
    private bool _isDeleting;
    [ObservableProperty]
    private string _displayName = "";
    [ObservableProperty]
    private string _errorMessage = "";

    public string ConfigName { get; }
    public bool CanDelete => _scheduleService.ScheduleConfig.Configs.Count > 1;

    public EditScheduleConfigWindowViewModel(ScheduleService scheduleService, Window window)
    {
        _scheduleService = scheduleService;
        _window = window;
        _configId = scheduleService.ScheduleConfig.CurrentConfig;
        ConfigName = scheduleService.ScheduleConfig.Configs[_configId].Name;
        DisplayName = ConfigName;
    }

    private void ShowMode(bool choosing, bool renaming, bool deleting)
    {
        ErrorMessage = "";
        IsChoosing = choosing;
        IsRenaming = renaming;
        IsDeleting = deleting;
    }

    [RelayCommand]
    private void ChooseRename() => ShowMode(false, true, false);

    [RelayCommand]
    private void ChooseDelete()
    {
        if (!CanDelete)
        {
            ErrorMessage = "You can't delete your only schedule";
            return;
        }
        ShowMode(false, false, true);
    }

    [RelayCommand]
    private void Back() => ShowMode(true, false, false);

    [RelayCommand]
    private void Cancel() => _window.Close();

    [RelayCommand]
    private void SubmitRename()
    {
        string name = DisplayName.Trim();
        if (name.Length == 0)
        {
            ErrorMessage = "Please enter a name";
            return;
        }
        if (_scheduleService.ConfigNameExists(name, _configId))
        {
            ErrorMessage = "A schedule with this name already exists";
            return;
        }
        _scheduleService.RenameConfig(_configId, name);
        _window.Close();
    }

    [RelayCommand]
    private void ConfirmDelete()
    {
        _scheduleService.DeleteConfig(_configId);
        _window.Close();
    }
}
