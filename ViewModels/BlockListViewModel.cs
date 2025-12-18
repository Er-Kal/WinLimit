
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
    private ObservableCollection<Button> _recommendedApps;
    public BlockListViewModel()
    {
        // Initialize collections immediately to avoid NullReferenceExceptions in the View
        BlockItems = new ObservableCollection<BlockItem>();
        RecommendedApps = new ObservableCollection<Button>();

        // Call the async method without awaiting it
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        try 
        {
            SupabaseService supabase = new SupabaseService();
            
            // This await works here because we are in a method, not a constructor
            string name = await supabase.getSupabaseURL(); 

            // Update the collection on the UI thread (usually automatic in MVVM, but safe to do here)
            BlockItems.Add(new BlockItem(name, "This is a game"));
            BlockItems.Add(new BlockItem("App2.exe", "This is social media"));
            BlockItems.Add(new BlockItem("App3.exe", "This is time waster"));

            RecommendedApps.Add(new Button{Content="Steam"});
            RecommendedApps.Add(new Button{Content="Roblox"});
        }
        catch (Exception ex)
        {
            // Handle errors here (e.g., log them or show a user alert)
            // If you don't catch exceptions here, they will likely crash the app silently.
            Console.WriteLine($"Initialization failed: {ex.Message}");
        }
    }
    
}
