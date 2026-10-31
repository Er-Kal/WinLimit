using System;
using System.Collections.Generic;
using WinLimit.Models;

public class ScheduleConfig
{
    public Guid CurrentConfig { get; set; }
    public Dictionary<Guid, ScheduleDefinition> Configs { get; set; }
    public ScheduleConfig(Dictionary<Guid, ScheduleDefinition> configs)
    {
        this.Configs = configs;
    }
}