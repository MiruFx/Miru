using System.Threading.Tasks;

namespace Miru.Queuing
{
    public interface IQueueCleaner
    {
        Task Clear();
    }
}