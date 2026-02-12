
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
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
    private BlockItem? SelectedBlockedItem { get; set; }
    [ObservableProperty]
    private string _friendlyCustomName = "";
    [ObservableProperty]
    private string _executableCustomName = "";
    [ObservableProperty]
    private string _customDescription = "";
    public BlockListViewModel(AppBlockerService appBlockerService)
    {
        // Initialize collections immediately to avoid NullReferenceExceptions in the View
        BlockedItems = new ObservableCollection<BlockItem>();
        RecommendedApps = new ObservableCollection<BlockItem>();
        _appBlockerService = appBlockerService;

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

    // Need to get rid of this func when hooked to backend
    private void Initialize()
    {
        RecommendedApps.Add(new BlockItem("Roblox","RobloxPlayerBeta","Roblox player"));
        RecommendedApps.Add(new BlockItem("steam","Steam library app"));
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
        if (FriendlyCustomName.Length > 0)
        {
            app = new BlockItem(FriendlyCustomName,ExecutableCustomName,CustomDescription);
        }
        else
        {
            app = new BlockItem(ExecutableCustomName,CustomDescription);
        }
        _appBlockerService.AddApp(app);
    }
}
