using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WinLimit.Views;

public partial class EditScheduleConfigWindow : Window
{
    public EditScheduleConfigWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
