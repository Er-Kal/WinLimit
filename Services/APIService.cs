using dotenv.net;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WinLimit.Models;

public class APIService
{
    private readonly HttpClient _httpClient;
    private bool validToken = false;
    public APIService()
    {
        DotEnv.Load();
        string API_URL = Environment.GetEnvironmentVariable("API_URL") ?? "https://winlimit.eriksk.com/";
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new System.Uri(API_URL);
    }
    public HttpClient ReturnHTTPClient()
    {
        return _httpClient;
    }
    public void SetAuthToken(string? token)
    {
        if (token == null)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            return;
        }
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    public async Task<string?> GetUserProfile()
    {
        if (_httpClient.DefaultRequestHeaders.Authorization == null) return null;

        var response = await _httpClient.GetAsync("api/profile");
        var data = await response.Content.ReadAsStringAsync();
        return data;
    }
    public async Task<List<BlockItem>?> GetLatestBlockRecommendations()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/blockitem/latest");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<BlockItem>>(result);

            return data;
        }
        else
        {
            Console.WriteLine("Error fetching block recommendations: " + response.StatusCode);
            return null;
        }
    }

    public async Task updateProfileSchedule(string scheduleJson)
    {
        var payload = new { Schedules = scheduleJson };

        var response = await _httpClient.PatchAsJsonAsync("api/profile", payload);
    }

    public async Task updateProfileBlockItems(string blockItemsJson)
    {
        var payload = new { BlockedApps = blockItemsJson };

        var response = await _httpClient.PatchAsJsonAsync("api/profile", payload);
    }

    public async Task<string?> getProfileEmail()
    {
        var response = await _httpClient.GetFromJsonAsync<UserEmail>("api/auth/me");
        if (response == null)
            return null;
        return response.Email;
    }

    public async Task LogAppBlocked(string appName, string appFriendlyName)
    {
        var payload = new { AppName = appName, FriendlyAppName = appFriendlyName ?? appName };

        var json = JsonSerializer.Serialize(payload);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/log", content);
    }
}

public record UserEmail(string Email);
