
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WinLimit.Models;
using WinLimit.Services;

namespace WinLimit.ViewModels;

public partial class BlockListViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<BlockItem> _blockedItems;
    [ObservableProperty]
    private ObservableCollection<BlockItem> _recommendedApps;
    private AppBlockerService _appBlockerService;
    [ObservableProperty]
    private BlockItem? _selectedBlockedItem;
    [ObservableProperty]
    private string _friendlyCustomName = "";
    [ObservableProperty]
    private string _executableCustomName = "";
    [ObservableProperty]
    private string _customDescription = "";
    private APIService _apiService;
    private string[] safeApps = ["explorer", "svchost", "lsass", "csrss", "winlogon", "dwm", "taskmgr", "services", "smss", "wininit","winlimit","msmpeng","securityhealthsystray","windefend","wuauclt","usoclient","ctfmon","fontdrvhost","nvcontainer","amdow","system","magnify","narrator","osk"];
    public BlockListViewModel(AppBlockerService appBlockerService, APIService apiService)
    {
        // Initialize collections immediately to avoid NullReferenceExceptions in the View
        BlockedItems = new ObservableCollection<BlockItem>();
        RecommendedApps = new ObservableCollection<BlockItem>();
        _appBlockerService = appBlockerService;
        _apiService = apiService;
        _appBlockerService.OnBlockedAppsChanged += OnBlockedAppsChanged;

        OnBlockedAppsChanged();
        Initialize();
    }
    private void OnBlockedAppsChanged()
    {
        BlockedItems.Clear();
        foreach (BlockItem blockedApp in _appBlockerService.BlockedApps)
        {
            BlockedItems.Add(blockedApp);
        }
    }

    private async Task Initialize()
    {
        List<BlockItem> items = await _apiService.GetLatestBlockRecommendations();
        if (items == null) return;
        if (items.Count > 0)
        {
            foreach (var item in items)
            {
                if (!BlockedItems.Any(block => block.ExecutableName==item.ExecutableName))
                    RecommendedApps.Add(item);
            }
        }
    }
    [RelayCommand]
    private void RemoveBlockedItem()
    {
        if (SelectedBlockedItem != null)
        {
            _appBlockerService.RemoveApp(SelectedBlockedItem);
        }
    }
    [RelayCommand]
    private void AddBlockedItem(BlockItem app)
    {
        _appBlockerService.AddApp(app);
        RecommendedApps.Remove(app);
    }
    [RelayCommand]
    private void AddCustomApp()
    {
        BlockItem app;

        string execName = Path.GetFileNameWithoutExtension(ExecutableCustomName);
        if (FriendlyCustomName.Length > 0)
        {
            app = new BlockItem(FriendlyCustomName,execName,CustomDescription);
        }
        else
        {
            app = new BlockItem(execName,CustomDescription);
        }

        if (safeApps.Any(name => name.ToLower() == app.ExecutableName!.ToLower()))
        {
            return;
        }
        _appBlockerService.AddApp(app);
    }
}
