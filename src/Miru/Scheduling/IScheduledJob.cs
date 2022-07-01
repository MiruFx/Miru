using System.Threading.Tasks;

namespace Miru.Scheduling;

public interface IScheduledJob
{
    Task ExecuteAsync();
}