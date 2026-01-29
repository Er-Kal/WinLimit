using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;
using System;

namespace WinLimit.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _count = 0;

    [RelayCommand]
    private void IncrementClick()
    {
        
        int currentHour = DateTime.Now.Hour;
        Count = currentHour;
    }
}
