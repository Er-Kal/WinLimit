

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
    public event Action<string> OnAppBlocked;
    public event Action<bool> OnTrackingChanged;
    public event Action OnBlockedAppsChanged;
    public IReadOnlyList<BlockItem> BlockedApps => blockedApps.AsReadOnly();
    // Constructor
    public AppBlockerService()
    {
        blockedApps = new List<BlockItem>();
        AddApp(new BlockItem("NotePad","notepad", ""));
        StartLoop();
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
        OnBlockedAppsChanged?.Invoke();
    }
    // RemoveApp removes a "BlockItem" from the "blockedApps" list and triggers "OnBlockedAppsChanged" event
    public void RemoveApp(BlockItem app)
    {
        blockedApps.Remove(app);
        OnBlockedAppsChanged?.Invoke();
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
        tracking = false;
        TrackingChanged();
    }
    // Invokes "OnTrackingChanged", used for displaying state of loop
    public void TrackingChanged()
    {
        OnTrackingChanged?.Invoke(tracking);
    }
}