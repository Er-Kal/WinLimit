

using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WinLimit.Models;
using WinLimit.Views;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace WinLimit.Services;

public class AppBlockerService
{
    private bool tracking = false;
    private List<BlockItem> blockedApps;
    private readonly APIService _apiService;
    public event Action<string>? OnAppBlocked;
    public event Action<bool>? OnTrackingChanged;
    public event Action? OnBlockedAppsChanged;
    public IReadOnlyList<BlockItem> BlockedApps => blockedApps.AsReadOnly();
    public ScheduleService scheduleService;
    public LocalStorageService _localStorageService;
    public bool BlockingOverride = false;
    // Constructor
    private Window popUpWindow;
    public AppBlockerService(LocalStorageService localStorageService, APIService apiService)
    {
        scheduleService = new ScheduleService(localStorageService, apiService);
        _apiService = apiService;
        blockedApps = new List<BlockItem>();
        scheduleService.OnSchedulesChanged += OnSchedulesChanged;
        StartLoop();
        _localStorageService = localStorageService;
        this.LoadBlockedAppsLocal();
    }

    private void OnSchedulesChanged()
    {
        // Restart loop if schedule becomes active and we're not already tracking
        if (!tracking && scheduleService.IsScheduledBlocked())
        {
            StartLoop();
        }
    }
    // Blocked Apps List Changed
    public void BlockedAppsChanged()
    {
        OnBlockedAppsChanged?.Invoke(); 
        SaveBlockedApps();
    }
    // AppBlocked triggers the OnAppBlocked Event
    public void AppBlocked()
    {
        Dispatcher.UIThread.Post(() => OnAppBlocked?.Invoke("An app has been blocked"));
    }
    // AddApp adds a "BlockItem" to the "blockedApps" list and triggers "OnBlockedAppsChanged" event
    public void AddApp(BlockItem app)
    {
        blockedApps.Add(app);
        BlockedAppsChanged();
    }
    // RemoveApp removes a "BlockItem" from the "blockedApps" list and triggers "OnBlockedAppsChanged" event
    public void RemoveApp(BlockItem app)
    {
        blockedApps.Remove(app);
        BlockedAppsChanged();
    }
    // Loop logic to go through every process and kill it if any match with a blocked app
    // Will trigger "AppBlocked" event
    private void CheckAndKill()
    {
        Process[] processes = Process.GetProcesses();

        foreach (Process process in processes)
        {
            string processName = process.ProcessName.ToLower();
            var blockedApp = blockedApps.FirstOrDefault(b => b.ExecutableName?.ToLower() == processName);
            if (blockedApp != null)
            {
                try
                {
                    process.Kill();
                    AppBlocked();
                    _apiService.LogAppBlocked(blockedApp.ExecutableName, blockedApp.FriendlyName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
    // StartLoop starts the app blocking loop, calls "CheckAndKill" every 2 seconds
    // Calls TrackingChanged function
    public async void StartLoop()
    {
        tracking = true;
        TrackingChanged();
        while (tracking)
        {
            if (!scheduleService.IsScheduledBlocked() || BlockingOverride)
            {
                StopLoop();
                break;
            }
            try
            {
                CheckAndKill();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            await Task.Delay(2000);
        }
    }
    // StopLoop stops the app killing loop, calls "TrackingChanged" function
    public void StopLoop()
    {
        if (tracking) // Only update if state is actually changing
        {
            tracking = false;
            TrackingChanged();
        }
    }
    // Invokes "OnTrackingChanged", used for displaying state of loop
    public void TrackingChanged()
    {
        OnTrackingChanged?.Invoke(tracking);
    }

    public void SaveBlockedApps()
    {
        JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(blockedApps, jsonOptions);
        _localStorageService.SaveBlockedApps(jsonString);
        _apiService.updateProfileBlockItems(jsonString);
    }
    public void LoadBlockedAppsLocal()
    {
        List<BlockItem>? data = _localStorageService.LoadBlockedApps();
        LoadBlockedApps(data);
    }
    public void LoadBlockedApps(List<BlockItem>? data)
    {
        if (data == null) return;
        blockedApps = data;
        BlockedAppsChanged();
    }
    public void ClearRules()
    {
        blockedApps.Clear();
        BlockedAppsChanged();
        scheduleService.CreateNewDict();
    }
    public async Task LoadUserProfile()
    {
        string? data = await _apiService.GetUserProfile();
        if (data == null) return;
        UserProfile? userProfile = JsonSerializer.Deserialize<UserProfile>(data);
        if (userProfile == null) return;

        if (!string.IsNullOrWhiteSpace(userProfile.BlockedAppsSettings))
        {
            LoadBlockedApps(JsonSerializer.Deserialize<List<BlockItem>>(userProfile.BlockedAppsSettings));
        }
        if (!string.IsNullOrWhiteSpace(userProfile.ScheduleSettings))
        {
            scheduleService.LoadSchedules(JsonSerializer.Deserialize<Dictionary<string, List<ScheduleRule>>?>(userProfile.ScheduleSettings));
        }
    }

    public void ChangeOverrideState()
    {
        BlockingOverride = !BlockingOverride;
    }
}