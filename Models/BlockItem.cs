using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinLimit.Models
{
    public partial class BlockItem : ObservableObject
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private DateTime _timeAdded;

        public BlockItem(string name, string description)
        {
            _name = name;
            _description = description;
            _timeAdded = DateTime.Now;

            WrapPanel panel = new WrapPanel();

            panel.Children.Add(new Button { Content = "Hahaha" });
        }
    }
}
