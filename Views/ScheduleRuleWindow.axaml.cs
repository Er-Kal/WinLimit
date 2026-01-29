using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WinLimit
{
    public partial class ScheduleRuleWindow : Window
    {
        public ScheduleRuleWindow()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}