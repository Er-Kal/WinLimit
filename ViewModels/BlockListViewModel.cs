
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
    private ObservableCollection<string> _recommendedApps;
    private AppBlockerService _appBlockerService;
    private BlockItem? SelectedBlockedItem { get; set; }
    public BlockListViewModel(AppBlockerService appBlockerService)
    {
        // Initialize collections immediately to avoid NullReferenceExceptions in the View
        BlockedItems = new ObservableCollection<BlockItem>();
        RecommendedApps = new ObservableCollection<string>();
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

    // Need to get rid of this func
    private void Initialize()
    {
        RecommendedApps.Add("Steam");
        RecommendedApps.Add("Roblox");
        RecommendedApps.Add("Roblox");
        RecommendedApps.Add("asdfasdfasdfasdf");
        RecommendedApps.Add("Roblox");
        RecommendedApps.Add("asdfadsfasdf");
        RecommendedApps.Add("asdfasdf");
        RecommendedApps.Add("asdf");
        RecommendedApps.Add("adsf");
        RecommendedApps.Add("asdfasdfsafsadfdsad");
        RecommendedApps.Add("Roblox");
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
    private void AddBlockedItem(string name)
    {
        _appBlockerService.AddApp(new BlockItem(name, name, ""));
        RecommendedApps.Remove(name);
    }
}
