
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WinLimit.Models;
using WinLimit.Services;

namespace WinLimit.ViewModels;

public partial class BlockListViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<BlockItem> _blockItems;
    [ObservableProperty]
    private ObservableCollection<RecommendedAppBlock> _recommendedApps;

    private readonly SupabaseService _supabase;
    public BlockListViewModel(SupabaseService supabaseService)
    {
        // Initialize collections immediately to avoid NullReferenceExceptions in the View
        BlockItems = new ObservableCollection<BlockItem>();
        RecommendedApps = new ObservableCollection<RecommendedAppBlock>();
        _supabase = supabaseService;
        // Call the async method without awaiting it
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        try 
        {
            // This await works here because we are in a method, not a constructor
            string name = await _supabase.getFirstUserEmail(); 

            // Update the collection on the UI thread (usually automatic in MVVM, but safe to do here)
            BlockItems.Add(new BlockItem(name, "This is a game"));
            BlockItems.Add(new BlockItem("App2.exe", "This is social media"));
            BlockItems.Add(new BlockItem("App3.exe", "This is time waster"));

            RecommendedApps.Add(new RecommendedAppBlock("Steam",null));
            RecommendedApps.Add(new RecommendedAppBlock("Roblox",null));
            RecommendedApps.Add(new RecommendedAppBlock("Roblox",null));
            RecommendedApps.Add(new RecommendedAppBlock("asdfasdfasdfasdf",null));
            RecommendedApps.Add(new RecommendedAppBlock("Roblox",null));
            RecommendedApps.Add(new RecommendedAppBlock("asdfadsfasdf",null));
            RecommendedApps.Add(new RecommendedAppBlock("asdfasdf",null));
            RecommendedApps.Add(new RecommendedAppBlock("asdf",null));
            RecommendedApps.Add(new RecommendedAppBlock("adsf",null));
            RecommendedApps.Add(new RecommendedAppBlock("asdfasdfsafsadfdsad",null));
            RecommendedApps.Add(new RecommendedAppBlock("Roblox",null));
        }
        catch (Exception ex)
        {
            // Handle errors here (e.g., log them or show a user alert)
            // If you don't catch exceptions here, they will likely crash the app silently.
            Console.WriteLine($"Initialization failed: {ex.Message}");
        }
    }
    
}
