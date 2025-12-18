using Avalonia.Controls;
using Avalonia.Markup.Xaml;

public partial class PopUpWindow : Window
{
    public PopUpWindow()
    {
        InitializeComponent();
    }
    public PopUpWindow(string message)
    {
        this.FindControl<TextBlock>("Message").Text=message;
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}