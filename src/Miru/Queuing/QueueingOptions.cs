namespace Miru.Queuing;

public class QueueingOptions
{
    public string ConnectionString { get; set; }
    public string Prefix { get; set; } = "hangfire";
}