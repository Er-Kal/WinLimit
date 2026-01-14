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
        public string FriendlyName {get; set;}
        public string ExecutableName {get; set;}
        public string? Description {get; set;}
        public DateTime TimeAdded {get; set;}

        public BlockItem(string friendlyName, string executableName, string description)
        {
            FriendlyName=friendlyName;
            ExecutableName=executableName;
            Description = description;
            TimeAdded = DateTime.Now;
        }
        public BlockItem(string executableName, string description)
        {
            FriendlyName=executableName;
            ExecutableName=executableName;
            Description = description;
            TimeAdded = DateTime.Now;
        }
    }
}
