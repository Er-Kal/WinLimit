using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using WinLimit.Services;

namespace WinLimit.ViewModels;

public partial class LogoutPopUpWindowViewModel : ViewModelBase
{
    private Window _window;
    private readonly AppBlockerService _appBlockerService;
    public LogoutPopUpWindowViewModel(Window window, AppBlockerService appBlockerService)
    {
        _window = window;
        _appBlockerService = appBlockerService;
    }
    [RelayCommand]
    private void Yes()
    {
        _window.Close();   
    }
    [RelayCommand]
    private void No()
    {
        _appBlockerService.ClearRules();
        _appBlockerService.scheduleService.CreateNewDict();
        _window.Close();
    }
}