using CommunityToolkit.Mvvm.Input;
using WinLimit.Services;

namespace WinLimit.ViewModels;

public partial class PopUpWindowViewModel : ViewModelBase
{
    AppBlockerService _appBlockerService;
    public PopUpWindowViewModel(AppBlockerService appBlockerService)
    {
        _appBlockerService=appBlockerService;
    }
    [RelayCommand]
    private void StopLoop()
    {
        _appBlockerService.StopLoop();
    }
}