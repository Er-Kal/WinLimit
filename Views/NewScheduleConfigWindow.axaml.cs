using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WinLimit.Views;

public partial class NewScheduleConfigWindow : Window
{
    public NewScheduleConfigWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
