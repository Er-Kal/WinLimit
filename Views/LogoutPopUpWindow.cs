using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WinLimit.Views;

public partial class LogoutPopUpWindow : Window
{
    public LogoutPopUpWindow()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}