using dotenv.net;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using WinLimit;
using WinLimit.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class APIService
{
    private readonly HttpClient _httpClient;
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
    public void SetAuthToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
}