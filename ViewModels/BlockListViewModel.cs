
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
    private readonly SupabaseService _supabase;
    private BlockItem? SelectedBlockedItem {get;set;}
    public BlockListViewModel(SupabaseService supabaseService, AppBlockerService appBlockerService)
    {
        // Initialize collections immediately to avoid NullReferenceExceptions in the View
        BlockedItems = new ObservableCollection<BlockItem>();
        RecommendedApps = new ObservableCollection<string>();
        _supabase = supabaseService;
        _appBlockerService = appBlockerService;

        _appBlockerService.OnBlockedAppsChanged += OnBlockedAppsChanged;

        OnBlockedAppsChanged();

        // Call the async method without awaiting it
        _ = InitializeAsync();
    }
    private void OnBlockedAppsChanged()
    {
        BlockedItems.Clear();
        foreach (BlockItem blockedApp in _appBlockerService.BlockedApps)
        {
            BlockedItems.Add(blockedApp);
        }
    }

    private async Task InitializeAsync()
    {
        try 
        {
            // This await works here because we are in a method, not a constructor
            string name = await _supabase.getFirstUserEmail(); 

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
        catch (Exception ex)
        {
            // Handle errors here (e.g., log them or show a user alert)
            // If you don't catch exceptions here, they will likely crash the app silently.
            Console.WriteLine($"Initialization failed: {ex.Message}");
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
    private void AddBlockedItem(string name)
    {
        _appBlockerService.AddApp(new BlockItem(name,name, ""));
        RecommendedApps.Remove(name);
    }
}
