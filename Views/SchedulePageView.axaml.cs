using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using WinLimit.Models;
using WinLimit.ViewModels;

namespace WinLimit.Views;
public partial class SchedulePageView : UserControl
{
    private ScheduleRule? _draggedRule;
    private bool _draggingTop;
    private bool _changedDuringDrag;

    public SchedulePageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Handle_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Control handle || handle.DataContext is not ScheduleRule rule) return;
        if (!e.GetCurrentPoint(handle).Properties.IsLeftButtonPressed) return;
        _draggedRule = rule;
        _draggingTop = (handle.Tag as string) == "Top";
        _changedDuringDrag = false;
        e.Pointer.Capture(handle);
        e.Handled = true;
    }

    private void Handle_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (_draggedRule == null || sender is not Control handle) return;
        var panel = handle.FindAncestorOfType<SchedulePanel>();
        if (panel == null || panel.Bounds.Height <= 0) return;

        // Snap to the nearest whole hour under the pointer
        double y = e.GetPosition(panel).Y;
        int hour = (int)Math.Round(y / panel.Bounds.Height * 24.0);

        var others = panel.Children
            .Select(c => (c as ContentPresenter)?.Content as ScheduleRule)
            .Where(r => r != null && r != _draggedRule)
            .Select(r => r!)
            .ToList();

        if (_draggingTop)
        {
            // Can't go past the end of this rule or into the rule above it
            int min = others.Where(r => r.EndHour <= _draggedRule.StartHour).Select(r => r.EndHour).DefaultIfEmpty(0).Max();
            int newStart = Math.Clamp(hour, min, _draggedRule.EndHour - 1);
            if (newStart != _draggedRule.StartHour)
            {
                _draggedRule.StartHour = newStart;
                _changedDuringDrag = true;
            }
        }
        else
        {
            // Can't go before the start of this rule or into the rule below it
            int max = others.Where(r => r.StartHour >= _draggedRule.EndHour).Select(r => r.StartHour).DefaultIfEmpty(24).Min();
            int newEnd = Math.Clamp(hour, _draggedRule.StartHour + 1, max);
            if (newEnd != _draggedRule.EndHour)
            {
                _draggedRule.EndHour = newEnd;
                _changedDuringDrag = true;
            }
        }
        panel.InvalidateArrange();
        e.Handled = true;
    }

    private void Handle_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_draggedRule == null) return;
        e.Pointer.Capture(null);
        _draggedRule = null;
        e.Handled = true;
        if (_changedDuringDrag && DataContext is SchedulePageViewModel vm)
        {
            vm.RuleResized();
        }
    }
}
