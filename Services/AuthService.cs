using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
namespace WinLimit.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly LocalStorageService _localStorageService;
    public AuthService(LocalStorageService localStorageService)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5226/");
        _localStorageService = localStorageService;
    }
    
    public async Task<string?> LoginAsync(string email, string password)
    {
        AuthDto loginData = new AuthDto(email, password);
        var response = await _httpClient.PostAsJsonAsync("api/auth/login",loginData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            string? token = result?.Token;
            if (token!=null)
                _localStorageService.SaveToken(token);
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
            if (token!=null)
                _localStorageService.SaveToken(token);
            return token;
        }

        return response.ToString();
    }
}
public record AuthDto(string Email, string Password);