

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WinLimit.Models;
namespace WinLimit.Services;

public class AppBlockerService
{
    private bool tracking = false;
    private List<BlockItem> blockedApps;
    public event Action<string>? OnAppBlocked;
    public event Action<bool>? OnTrackingChanged;
    public event Action? OnBlockedAppsChanged;
    public IReadOnlyList<BlockItem> BlockedApps => blockedApps.AsReadOnly();
    public ScheduleService scheduleService;
    public LocalStorageService _localStorageService;
    // Constructor
    public AppBlockerService(LocalStorageService localStorageService)
    {
        scheduleService = new ScheduleService(localStorageService);
        blockedApps = new List<BlockItem>();
        scheduleService.OnSchedulesChanged += OnSchedulesChanged;
        StartLoop();
        _localStorageService = localStorageService;
        this.LoadBlockedApps();
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
        OnAppBlocked?.Invoke("An app has been blocked");
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
            if (blockedApps.Any(b => b.ExecutableName.ToLower() == processName))
            {
                try
                {
                    process.Kill();
                    AppBlocked();
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
            if (!scheduleService.IsScheduledBlocked())
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
        _localStorageService.SaveBlockedApps(blockedApps);
    }

    public void LoadBlockedApps()
    {
        List<BlockItem>? data = _localStorageService.LoadBlockedApps();
        if (data == null) return;
        blockedApps = data;
    }
}