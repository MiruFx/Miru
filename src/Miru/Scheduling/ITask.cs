namespace Miru.Scheduling
{
    public interface ITask
    {
        ITask EveryMonday();
        ITask EveryDay();
    }
}