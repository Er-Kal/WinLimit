
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinLimit.Views;

namespace WinLimit.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage = null!;
    private readonly HomeViewModel _homePage;
    private readonly SchedulePageViewModel _schedulePage;

    public MainWindowViewModel()
    {
        _homePage = new HomeViewModel();
        _schedulePage = new SchedulePageViewModel();
        CurrentPage=_homePage;
    }

    [RelayCommand]
    private void ShowHome()
    {
        CurrentPage = _homePage;
    }
    [RelayCommand]
    private void ShowSchedule()
    {
        CurrentPage=_schedulePage;
    }
}
