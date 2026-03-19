using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using WinLimit.Models;
namespace WinLimit.Views;
public class SchedulePanel : Panel
{
    protected override Size ArrangeOverride(Size finalSize)
    {
        foreach (var child in Children)
        {
            var rule = (child as ContentPresenter)?.Content as ScheduleRule;
            if (rule == null) continue;

            double top = (rule.StartHour / 24.0) * finalSize.Height;
            double height = (rule.Duration / 24.0) * finalSize.Height;
            child.Arrange(new Rect(0, top, finalSize.Width, height));
        }
        return finalSize;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        foreach (var child in Children)
            child.Measure(availableSize);
        return availableSize;
    }
}