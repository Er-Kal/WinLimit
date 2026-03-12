using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WinLimit.Models;
namespace WinLimit.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly LocalStorageService _localStorageService;
    private readonly APIService _apiService;
    private string? username = null;
    public AuthService(LocalStorageService localStorageService, APIService apiService)
    {
        _httpClient = apiService.ReturnHTTPClient();
        _localStorageService = localStorageService;
        _apiService = apiService;
        LoadLocalToken();
    }
    
    public async Task<string?> LoginAsync(string email, string password)
    {
        AuthDto loginData = new AuthDto(email, password);
        var response = await _httpClient.PostAsJsonAsync("api/auth/login",loginData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            string? token = result?.Token;
            if (token != null)
            {
                _localStorageService.SaveToken(token);
                _apiService.SetAuthToken(token);
            }
            return token;
        }

        return null;
    }
    public async Task<string?> RegisterAsync(string email, string password)
    {
        var registerData = new AuthDto(email, password);
        var response = await _httpClient.PostAsJsonAsync("api/auth/register",registerData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            string? token = result?.Token;
            if (token != null)
            {
                _localStorageService.SaveToken(token);
                _apiService.SetAuthToken(token);
            }
            return token;
        }

        return response.ToString();
    }

    public void LoadLocalToken()
    {
        string? token = _localStorageService.LoadToken();
        if (token == null) return;
        _apiService.SetAuthToken(token);
    }

    public void ClearToken()
    {
        string? token = null;
        _apiService.SetAuthToken(null);
        _localStorageService.DeleteToken();
    }

    public string? GetToken()
    {
        try
        {
            if (_httpClient.DefaultRequestHeaders.Authorization == null) return null;
            return _httpClient.DefaultRequestHeaders.Authorization.Parameter;
        }
        catch( Exception e)
        {
            return null;
        }
    }

    public async Task<string?> GetUserEmail()
    {
        var response = await _httpClient.GetAsync("api/auth/me");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<EmailResponse>();
        return result?.Email;
    }
}
public record AuthDto(string Email, string Password);
public record EmailResponse(string Email);