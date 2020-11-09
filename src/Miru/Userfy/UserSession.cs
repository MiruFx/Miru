using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Miru.Userfy
{
    public class WebUserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserfyOptions _options;
        private readonly ILogger<IUserSession> _logger;

        public WebUserSession(IHttpContextAccessor httpContextAccessor, UserfyOptions options, ILogger<IUserSession> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options;
            _logger = logger;
        }

        public void Login(IUser user, bool remember)
        {
            // https://github.com/dotnet/aspnetcore/blob/master/src/Security/Authentication/WsFederation/samples/WsFedSample/Startup.cs
            
            // TODO: check if httpcontext is present, otherwise throw Exception explaining user is not possible
            // sign in user without a http request. Are you using out of a asp.net request?

            // TODO: make check above on all methods

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Display),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.Add(_options.RememberFor),
                IsPersistent = false,
                AllowRefresh = false
            };
            
            claimsIdentity.AddClaims(claims);

            var userPrincipal = new ClaimsPrincipal(claimsIdentity);

            _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);
            
            _logger.LogDebug($"Logged in User #{user.Id} - {user.Display}");
        }

        public void Logout()
        {
            if (IsLogged)
            {
                var userId = CurrentUserId;
                var userDisplay = Display;
                
                _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                _logger.LogDebug($"Logged out User #{userId} - {userDisplay}");
            }
        }

        public long CurrentUserId
        {
            // TODO: Should return 0? or should be nullable? what about apps with other kind of ids, like guids?
            get
            {
                if (IsLogged == false)
                    return 0;

                return Convert.ToInt64(GetClaim(ClaimTypes.Sid));
            }
        }

        public string Display => GetClaim(ClaimTypes.Name);

        protected string GetClaim(string claimType)
        {
            var userPrincipal = _httpContextAccessor.HttpContext.User;

            // Look for the LastChanged claim.
            return (
                from c in userPrincipal.Claims
                where c.Type == claimType
                select c.Value
            ).FirstOrDefault();
        }

        public bool IsLogged
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                    return false;
                    
                return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            }
        }

        public bool IsAnonymous => !IsLogged;
    }
}
