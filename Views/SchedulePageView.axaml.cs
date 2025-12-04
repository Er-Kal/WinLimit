using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WinLimit.Views;
public partial class SchedulePageView : UserControl
{
    public SchedulePageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}