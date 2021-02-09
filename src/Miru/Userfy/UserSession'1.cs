using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Miru.Domain;

namespace Miru.Userfy
{
    public class UserSession<TUser> : IUserSession<TUser> where TUser : UserfyUser
    {
        private readonly IUserSession _userSession;
        private readonly DbContext _db;
        private readonly ILogger<UserSession<TUser>> _logger;

        public UserSession(IUserSession userSession, DbContext db, ILogger<UserSession<TUser>> logger)
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
                
                await LogoutAsync();
                
                throw;
            }
        }

        public async Task<SignInResult> LoginAsync(string userName, string password, bool remember = false)
        {
            return await _userSession.LoginAsync(userName, password, remember);
        }

        public async Task LogoutAsync()
        {
            await _userSession.LogoutAsync();
        }

        public long CurrentUserId => _userSession.CurrentUserId.ToLong();

        public string Display => _userSession.Display;

        public bool IsLogged => _userSession.IsLogged;

        public bool IsAnonymous => _userSession.IsAnonymous;
    }
}