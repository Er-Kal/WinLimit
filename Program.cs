using Avalonia;
using System;
using System.Threading;

namespace WinLimit;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    private static Mutex? _mutex;
    [STAThread]
    public static void Main(string[] args) //=> BuildAvaloniaApp()
                                           //.StartWithClassicDesktopLifetime(args);
    {
        const string mutexName = "WinLimitAppMutex";
        _mutex = new Mutex(true, mutexName, out bool createdNew);
        if (!createdNew)
        {
            // This means that the program is already running
            return;
        }

        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        finally
        {
            _mutex.ReleaseMutex();
            _mutex.Dispose();
        }

    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .With(new Win32PlatformOptions
            {
                DpiAwareness = Win32DpiAwareness.PerMonitorDpiAware,
            })
            .WithInterFont()
            .LogToTrace();
}
