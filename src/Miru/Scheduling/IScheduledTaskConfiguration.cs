using Quartz;

namespace Miru.Scheduling
{
    public interface IScheduledTaskConfiguration
    {
        void  Configure(IServiceCollectionQuartzConfigurator config);
    }
}