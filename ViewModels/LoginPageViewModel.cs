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
    private string _email="";
    [ObservableProperty]
    private string _password="";
    [ObservableProperty]
    private string _jwtToken = "";
    public LoginPageViewModel()
    {
        _authService = new AuthService();
    }
    [RelayCommand]
    public async Task Register()
    {
        try
        {
            string? response = await _authService.RegisterAsync(_email,_password);
            if (response!=null)
                JwtToken=response;
            else
                JwtToken="There was a problem";
        }
        catch (Exception ex)
        {
            JwtToken=$"Error: {ex.Message}";
        }
    }
}