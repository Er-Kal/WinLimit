
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
    [ObservableProperty]
    private BlockItem? _selectedBlockedItem;
    [ObservableProperty]
    private string _friendlyCustomName = "";
    [ObservableProperty]
    private string _executableCustomName = "";
    [ObservableProperty]
    private string _customDescription = "";
    private APIService _apiService;
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
        var items = await _apiService.GetLatestBlockRecommendations();
        if (items == null) return;
        if (items.Count > 0)
        {
            foreach (var item in items)
            {
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
