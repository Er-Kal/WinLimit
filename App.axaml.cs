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
using Avalonia.Controls;

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

        // Create App Blocker service

        collection.AddSingleton<LocalStorageService>();

        collection.AddSingleton<AppBlockerService>();

        collection.AddSingleton<APIService>();

        // Register View Models that use Singletons
        collection.AddTransient<MainWindowViewModel>();

        // Build DI provider
        Services = collection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            var mainViewModel = Services.GetRequiredService<MainWindowViewModel>();

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

            desktop.MainWindow.Closing += OnWindowClosing;
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

    private void OnWindowClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true; // Cancel the closing event to prevent the window from closing immediately
        if (sender is Window window)
        {
            window.Hide(); // Hide the window instead of closing it
        }
    }

    private void TrayIcon_Clicked(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = desktop.MainWindow;
            window?.Show();
            window?.Activate();
            window.WindowState=WindowState.Normal;
        }
    }

    private void OnQuit_Clicked(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow.Closing -= OnWindowClosing;
            desktop.Shutdown();
        }
    }
}