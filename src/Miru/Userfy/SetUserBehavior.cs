using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Miru.Behaviors.BelongsToUser;
using Miru.Domain;
using Miru.Pagination;

namespace Miru.Userfy
{
    public class SetUserBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUserSession _userSession;

        public SetUserBehavior(IUserSession userSession)
        {
            _userSession = userSession;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is IBelongsToUser belongToUserRequest && _userSession.IsLogged)
            {
                belongToUserRequest.UserId = _userSession.CurrentUserId.ToLong();
            }
            
            return await next();
        }
    }
}