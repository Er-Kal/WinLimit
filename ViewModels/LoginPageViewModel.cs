using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinLimit.Services;

namespace WinLimit.ViewModels;

public partial class LoginPageViewModel : ViewModelBase
{
    AuthService _authService;
    [ObservableProperty]
    private string _username = "";
    [ObservableProperty]
    private string _password = "";
    [ObservableProperty]
    private string? _jwtToken = "";
    public LoginPageViewModel(LocalStorageService localStorageService, APIService apiService)
    {
        _authService = new AuthService(localStorageService, apiService);
        _jwtToken=localStorageService.LoadToken();
    }
    [RelayCommand]
    public async Task Register()
    {
        try
        {
            string? response = await _authService.RegisterAsync(Username, Password);
            if (response != null)
                JwtToken = response;
            else
                JwtToken = "There was a problem";
        }
        catch (Exception ex)
        {
            JwtToken = $"Error: {ex.Message}";
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
                JwtToken = response;
                return;
            }
            JwtToken = "There was a problem";
        }
        catch (Exception ex)
        {
            JwtToken = ex.Message;
        }
    }
}