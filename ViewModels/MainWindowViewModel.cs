using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinLimit.Views;
using WinLimit.Services;

namespace WinLimit.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage = null!;
    private readonly HomeViewModel _homePage;
    private readonly SchedulePageViewModel _schedulePage;
    private readonly BlockListViewModel _blockListPage;
    private readonly AppBlockerService _appBlockerService;
    private readonly LocalStorageService _localStorageService;
    private readonly APIService _apiService;
    private readonly AuthService _authService;
    private readonly LoginPageViewModel _loginPage;
    private readonly Bitmap _shieldOn = new Bitmap(AssetLoader.Open(new System.Uri("avares://WinLimit/Assets/onindicator.png")));
    private readonly Bitmap _shieldOff = new Bitmap(AssetLoader.Open(new System.Uri("avares://WinLimit/Assets/offindicator.png")));
    [ObservableProperty]
    private Bitmap? _currentIcon;

    public MainWindowViewModel(AppBlockerService appBlockerService, LocalStorageService localStorageService, APIService apiService)
    {
        _appBlockerService = appBlockerService;
        _localStorageService = localStorageService;
        _apiService = apiService;
        _authService = new AuthService(_localStorageService, _apiService);
        _homePage = new HomeViewModel();
        _schedulePage = new SchedulePageViewModel(_appBlockerService.scheduleService);
        _blockListPage = new BlockListViewModel(_appBlockerService, _apiService);
        _loginPage = new LoginPageViewModel(_authService,_appBlockerService);
        CurrentPage = _homePage;
        appBlockerService.OnAppBlocked += OnAppBlocked;
        appBlockerService.OnTrackingChanged += OnTrackingChanged;
        OnTrackingChanged(_appBlockerService.scheduleService.IsScheduledBlocked());
    }

    [RelayCommand]
    private void ShowHome()
    {
        CurrentPage = _homePage;
    }
    [RelayCommand]
    private void ShowSchedule()
    {
        CurrentPage = _schedulePage;
    }
    [RelayCommand]
    private void ShowBlockList()
    {
        CurrentPage = _blockListPage;
    }
    [RelayCommand]
    private void ShowLoginPage()
    {
        CurrentPage = _loginPage;
    }
    [RelayCommand]
    private void PopUpTest()
    {
        PopUpWindow window = new PopUpWindow("Hello");
        window.DataContext = new PopUpWindowViewModel(_appBlockerService);
        window.Show();
    }
    private void OnAppBlocked(string message)
    {
        PopUpWindow window = new PopUpWindow("App has been blocked");
        window.DataContext = new PopUpWindowViewModel(_appBlockerService);
        window.Show();
    }
    private void OnTrackingChanged(bool state)
    {
        if (state)
        {
            CurrentIcon = _shieldOn;
        }
        else
        {
            CurrentIcon = _shieldOff;
        }
    }
}
