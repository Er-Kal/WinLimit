using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using WinLimit.ViewModels;
using WinLimit.Views;
using WinLimit.Services;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace WinLimit;

public partial class App : Application
{
    public static IServiceProvider? Services {get; private set;}
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {   
        //Create the collection of services
        var collection = new ServiceCollection();

        // Create the database service
        collection.AddSingleton<SupabaseService>();

        // Register View Models that use Singletons
        collection.AddTransient<MainWindowViewModel>();

        // Build DI provider
        Services = collection.BuildServiceProvider();

        var supabase = Services.GetRequiredService<SupabaseService>();
        _ = supabase.InitializeAsync();


        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            /*desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };*/

            var mainViewModel = Services.GetRequiredService<MainWindowViewModel>();

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}