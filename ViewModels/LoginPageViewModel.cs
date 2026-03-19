using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
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
    [ObservableProperty]
    private string _passwordCheck = "";
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
    [RelayCommand]
    public void Override()
    {
        if (!_appBlockerService.BlockingOverride)
        {
            _appBlockerService.AppBlocked();
        }
        else
        {
            _appBlockerService.ChangeOverrideState();
        }
    }
    partial void OnPasswordChanged(string value)
    {
        string output = "";
        if (Password.Length < 8)
        {
            output = "Password must be at least 8 characters long.";
        }
        if (!Password.Any(char.IsUpper))
        {
            output += " Password must contain at least one uppercase letter." + Environment.NewLine;
        }
        if (!Password.Any(char.IsNumber))
        {
            output+="Password must contain at least one number." + Environment.NewLine;
        }
        if (!Password.Any(c => !char.IsLetterOrDigit(c)))
        {
            output += "Password must contain at least one special character." + Environment.NewLine;
        }
        PasswordCheck = output;
    }
}