namespace Miru.Scheduling;

public class ScheduledJobOptions
{
    public bool SkipScheduling { get; set; }
    public bool SkipRunning { get; set; }
    public string QueueName { get; set; }
}