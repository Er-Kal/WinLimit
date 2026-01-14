/*public async Task<string> LoginAsync(string email, string password)
{
    var loginData = new { Email = email, Password = password };
    var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginData);
    
    if (response.IsSuccessStatusCode)
    {
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        // Save this token!
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", result.Token);
        return "Success";
    }
    return "Login Failed";
}*/

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WinLimit.Models;
namespace WinLimit.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    public AuthService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5226/");
    }
    
    public async Task<string?> LoginAsync(string email, string password)
    {
        AuthDto loginData = new AuthDto(email, password);
        var response = await _httpClient.PostAsJsonAsync("api/auth/login",loginData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            return result?.Token;
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
            return result?.Token;
        }

        return response.ToString();
    }
}
public record AuthDto(string Email, string Password);