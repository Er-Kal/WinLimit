using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WinLimit.Views;
public partial class LoginPageView : UserControl
{
    public LoginPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}