namespace Miru.Behaviors.UserStamp;

public interface IUserStamped : IUserStamped<long>
{
}

public interface IUserStamped<TIdType>
{
    TIdType CreatedById { get; set; }
        
    TIdType UpdatedById { get; set; }
}