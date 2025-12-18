
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinLimit.Services;
using WinLimit.Views;

namespace WinLimit.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage = null!;
    private readonly HomeViewModel _homePage;
    private readonly SchedulePageViewModel _schedulePage;
    private readonly BlockListViewModel _blockListPage;

    public MainWindowViewModel(SupabaseService supabaseService)
    {
        _homePage = new HomeViewModel();
        _schedulePage = new SchedulePageViewModel();
        _blockListPage = new BlockListViewModel(supabaseService);
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
    [RelayCommand]
    private void ShowBlockList()
    {
        CurrentPage=_blockListPage;
    }
}
