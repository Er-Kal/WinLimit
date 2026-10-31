using System;

namespace WinLimit.Models;

/// <summary>A selectable schedule config (id + display name) for the Schedule page drop down.</summary>
public record ScheduleOption(Guid Id, string Name)
{
    /// <summary>Sentinel entry shown at the end of the drop down to create a new config.</summary>
    public static readonly ScheduleOption NewConfig = new(Guid.Empty, "...new");

    public bool IsNewConfig => Id == Guid.Empty;
}
