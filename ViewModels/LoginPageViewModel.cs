using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinLimit.Services;
using WinLimit.Views;

namespace WinLimit.ViewModels;

public partial class LoginPageViewModel : ViewModelBase
{
    private AuthService _authService;
    [ObservableProperty]
    private string _username = "";
    [ObservableProperty]
    private string _password = "";
    [ObservableProperty]
    private string? _loginPageErrorMessage = "";
    [ObservableProperty]
    private string? _userEmail = "";
    private readonly AppBlockerService _appBlockerService;
    [ObservableProperty]
    private bool _isLoggedIn = false;
    [ObservableProperty]
    private bool _isLoggedOut = true;
    public LoginPageViewModel(AuthService authService, AppBlockerService appBlockerService)
    {
        _authService = authService;
        _appBlockerService = appBlockerService;
        LoadUser();
    }
    private async Task LoadUser()
    {
        UserEmail = await _authService.GetUserEmail();
         if (UserEmail != null)
        {
            IsLoggedIn = true;
            IsLoggedOut = false;
        }
         else
        {
            IsLoggedIn = false;
            IsLoggedOut = true;
        }
    }
    [RelayCommand]
    public async Task Register()
    {
        try
        {
            string? response = await _authService.RegisterAsync(Username, Password);
            await LoadUser();
            if (response == null)
                LoginPageErrorMessage = "There was a problem";
        }
        catch (Exception ex)
        {
            LoginPageErrorMessage = $"Error: {ex.Message}";
        }
    }
    [RelayCommand]
    public async Task Login()
    {
        try
        {
            string? response = await _authService.LoginAsync(Username, Password);
            if (response != null)
            {
                await _appBlockerService.LoadUserProfile();
                await LoadUser();
                return;
            }
            LoginPageErrorMessage = "There was a problem";
        }
        catch (Exception ex)
        {
            LoginPageErrorMessage = ex.Message;
        }
    }
    [RelayCommand]
    public void Logout()
    {
        _authService.ClearToken();
        LoginPageErrorMessage = "";
        UserEmail = "";
        IsLoggedIn = false;
        IsLoggedOut = true;
        LogoutPopUpWindow wind = new LogoutPopUpWindow();
        wind.DataContext = new LogoutPopUpWindowViewModel(wind, _appBlockerService);
        wind.Show();
    }
}