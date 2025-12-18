

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AppBlockerService
{
    private bool tracking;
    private List<string> blockedApps;
    public event Action<string> OnAppBlocked;
    public AppBlockerService()
    {
        
    }
    public void AppBlocked()
    {
        OnAppBlocked?.Invoke("An app has been blocked");
    }
    public void AddApp(string appName)
    {
        blockedApps.Append(appName);
    }

    public void RemoveApp(string appName)
    {
        blockedApps.Remove(appName);
    }
}