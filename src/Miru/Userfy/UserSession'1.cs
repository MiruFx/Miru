using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Miru.Domain;

namespace Miru.Userfy
{
    public class UserSession<TUser> : IUserSession<TUser> 
        where TUser : class, IEntity, IUser
    {
        private readonly IUserSession _userSession;
        private readonly DbContext _db;
        private readonly ILogger<IUserSession<TUser>> _logger;

        public UserSession(IUserSession userSession, DbContext db, ILogger<IUserSession<TUser>> logger)
        {
            _userSession = userSession;
            _db = db;
            _logger = logger;
        }

        public async Task<TUser> GetUserAsync()
        {
            try
            {
                if (CurrentUserId == 0)
                    return null;
                
                return await _db.Set<TUser>().ByIdOrFailAsync(CurrentUserId);
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation($"{ex.Message}. Maybe the was some inconsistency with auth cookie and the database. Logging out current user");
                
                Logout();
                
                throw;
            }
        }

        public void Login(IUser user, bool remember = false) => _userSession.Login(user, remember);

        public void Logout() => _userSession.Logout();

        public long CurrentUserId => _userSession.CurrentUserId;

        public string Display => _userSession.Display;

        public bool IsLogged => _userSession.IsLogged;

        public bool IsAnonymous => _userSession.IsAnonymous;
    }
}