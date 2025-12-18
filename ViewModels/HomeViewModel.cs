using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;

namespace WinLimit.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _count = 0;

    [RelayCommand]
    private void IncrementClick()
    {
        Count++;
    }
}
