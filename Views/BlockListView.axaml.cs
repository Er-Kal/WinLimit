using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WinLimit.Views;
public partial class BlockListView : UserControl
{
    public BlockListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}