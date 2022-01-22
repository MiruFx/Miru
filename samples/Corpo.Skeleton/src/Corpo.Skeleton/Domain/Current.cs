using Corpo.Skeleton.Config;

namespace Corpo.Skeleton.Domain;

public class Current
{
    public bool Loaded { get; set; }
    public bool IsAuthenticated { get; set; }
    public User User { get; set; }
    public AppOptions AppOptions { get; set; }
}