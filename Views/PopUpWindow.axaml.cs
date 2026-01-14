using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WinLimit
{
    public partial class PopUpWindow : Window
    {
        public PopUpWindow()
        {
            InitializeComponent();
        }
        public PopUpWindow(string message)
        {
            InitializeComponent();
            this.FindControl<TextBlock>("Message")!.Text = message;
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}